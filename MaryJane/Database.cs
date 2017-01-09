using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using libWiiSharp;
using MaryJane.Structs;
using Newtonsoft.Json;

namespace MaryJane
{
    public class Database
    {
        private static List<WiiUTitle> DbObject { get; set; }
        private static string TitleKeys => "https://wiiu.titlekeys.com";
        private static string DatabaseFile => "database.json";

        private event EventHandler DownloadTitleCompleted;

        public void Update()
        {
            try
            {
                if (DateTime.Now > new FileInfo(DatabaseFile).LastWriteTime.AddDays(1))
                {
                    var data = Network.DownloadData(TitleKeys + "/json");

                    if (File.Exists(DatabaseFile))
                        File.Delete(DatabaseFile);
                    File.WriteAllBytes(DatabaseFile, data);
                }
            }
            catch (Exception e)
            {
                Toolbelt.AppendLog($"{e.Message}\n{e.StackTrace}");
            }

            PopulateTables();
            Toolbelt.AppendLog("Database Update to date!");
        }

        public void UpdateGame(string titleId, string fullPath)
        {
            var game = FindByTitleId(titleId);

            game.TitleID = game.TitleID.Replace("00050000", "0005000e");

            DownloadTitleCompleted += (outputDir, args) =>
            {
                var outDir = (string) outputDir;
                
                Toolbelt.MoveDirectory(outDir, fullPath);
                
                Toolbelt.Form1.listBox1.Invoke(new Action(() =>
                {
                    Toolbelt.Form1.listBox1.Enabled = true;
                }));
            };

            System.Threading.Tasks.Task.Run(() => DownloadTitle(game));
        }

        public WiiUTitle Find(string game_name)
        {
            if (game_name == null) return new WiiUTitle();

            var fullPath = Path.Combine(Toolbelt.Settings.TitleDirectory, game_name);
            var entries = Directory.GetFileSystemEntries(fullPath, "meta.xml", SearchOption.AllDirectories);
            if (entries.Length <= 0) return new WiiUTitle {Name = "No meta.xml found!"};

            var xml = new XmlDocument();
            xml.Load(entries[0]);
            var titleId_tag = xml.GetElementsByTagName("title_id");

            if (titleId_tag.Count > 0)
            {
                var titleId = titleId_tag[0].InnerText;

                var title = DbObject.Find(t => t.TitleID.ToLower() == titleId.ToLower());

                return title;
            }

            return new WiiUTitle {Name = "NULL"};
        }

        private WiiUTitle FindByTitleId(string titleId)
        {
            return titleId == null ? new WiiUTitle() : DbObject.Find(t => t.TitleID.ToLower() == titleId.ToLower());
        }

        private static void PopulateTables()
        {
            var json = File.ReadAllText(DatabaseFile);
            DbObject = JsonConvert.DeserializeObject<List<WiiUTitle>>(json);
            DbObject.RemoveAll(t => t.ToString().Contains("()"));
        }

        private string DownloadTitle(WiiUTitle wiiUTitle)
        {
            var WII_TIK_URL = "https://wiiu.titlekeys.com/ticket/";
            var WII_NUS_URL = "http://nus.cdn.shop.wii.com/ccs/download/";
            var WII_WUP_URL = "http://ccs.cdn.wup.shop.nintendo.net/ccs/download/";

            var outputDir = Path.Combine("temp");

            Toolbelt.AppendLog($"Downloading Title {wiiUTitle.TitleID} v[Latest]...");
            
            string titleUrl = $"{WII_WUP_URL}{wiiUTitle.TitleID}/";
            string titleUrl2 = $"{WII_NUS_URL}{wiiUTitle.TitleID}/";

            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
            if (!Directory.Exists(Path.Combine(outputDir, wiiUTitle.ToString())))
                Directory.CreateDirectory(Path.Combine(outputDir, wiiUTitle.ToString()));
            outputDir = Path.Combine(outputDir, wiiUTitle.ToString());

            //Download TMD
            var tmdFile = "tmd";
            var ticket = "ticket";
            Toolbelt.AppendLog("  - Downloading TMD...");
            TMD tmd;
            byte[] tmdFileWithCerts;
            try
            {
                tmdFileWithCerts = Network.DownloadData(titleUrl + tmdFile);
                tmd = TMD.Load(tmdFileWithCerts);
            }
            catch (Exception ex)
            {
                Toolbelt.AppendLog("   + Downloading TMD Failed...");
                throw new Exception("Downloading TMD Failed:\n" + ex.Message);
            }

            //Parse TMD
            Toolbelt.AppendLog("  - Parsing TMD...");

            Toolbelt.AppendLog($"    + Title Version: {tmd.TitleVersion}");
            Toolbelt.AppendLog($"    + {tmd.NumOfContents} Contents");

            File.WriteAllBytes(Path.Combine(outputDir, tmdFile), tmdFileWithCerts);

            //Download cetk
            Toolbelt.AppendLog("  - Downloading Ticket...");
            try
            {
                Network.DownloadFile(Path.Combine(titleUrl, "cetk"), Path.Combine(outputDir, ticket));
            }
            catch (Exception ex)
            {
                try
                {
                    if (wiiUTitle.Ticket == "1")
                    {
                        var cetkUrl = $"{WII_TIK_URL}{wiiUTitle.TitleID.ToLower()}.tik";
                        Network.DownloadFile(cetkUrl, Path.Combine(outputDir, ticket));
                    }
                }
                catch
                {
                    Toolbelt.AppendLog("   + Downloading Ticket Failed...");
                    throw new Exception("Downloading Ticket Failed:\n" + ex.Message);
                }
            }

            // Parse Ticket

            if (File.Exists(Path.Combine(outputDir, ticket)))
            {
                Toolbelt.AppendLog("   + Parsing Ticket...");
                Ticket.Load(Path.Combine(outputDir, ticket));
            }
            else
            {
                Toolbelt.AppendLog("   + Ticket Unavailable...");
                throw new Exception("   + Ticket Unavailable...");
            }

            var encryptedContents = new string[tmd.NumOfContents];

            //Download Content
            for (var i = 0; i < tmd.NumOfContents; i++)
            {
                var size = Toolbelt.SizeSuffix((long) tmd.Contents[i].Size);
                Toolbelt.AppendLog($"  - Downloading Content #{i + 1} of {tmd.NumOfContents}... ({size} bytes)");

                var contentPath = Path.Combine(outputDir, tmd.Contents[i].ContentID.ToString("x8"));
                if (Toolbelt.IsValid(tmd.Contents[i], contentPath))
                {
                    Toolbelt.AppendLog("   + Using Local File, Skipping...");
                    continue;
                }

                try
                {
                    var downloadUrl = titleUrl + tmd.Contents[i].ContentID.ToString("x8");
                    var outputdir = Path.Combine(outputDir, tmd.Contents[i].ContentID.ToString("x8"));
                    Network.DownloadFile(downloadUrl, outputdir);

                    encryptedContents[i] = tmd.Contents[i].ContentID.ToString("x8");
                }
                catch (Exception ex)
                {
                    Toolbelt.AppendLog($"  - Downloading Content #{i + 1} of {tmd.NumOfContents} failed...");
                    throw new Exception("Downloading Content Failed:\n" + ex.Message);
                }
            }

            Toolbelt.AppendLog("  - Decrypting Content...");
            Toolbelt.CDecrypt(outputDir);

            Toolbelt.AppendLog("  - Deleting Encrypted Contents...");
            foreach (TMD_Content t in tmd.Contents)
                if (File.Exists(Path.Combine(outputDir, t.ContentID.ToString("x8"))))
                    File.Delete(Path.Combine(outputDir, t.ContentID.ToString("x8")));

            Toolbelt.AppendLog("  - Deleting TMD and Ticket...");
            File.Delete(Path.Combine(outputDir, "tmd"));
            File.Delete(Path.Combine(outputDir, "ticket"));

            Toolbelt.AppendLog($"Downloading Title {wiiUTitle.TitleID} v{tmd.TitleVersion} Finished.");

            DownloadTitleCompleted?.Invoke(outputDir, EventArgs.Empty);
            return outputDir;
        }
    }
}