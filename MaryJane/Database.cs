using libWiiSharp;
using MaryJane.Structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace MaryJane
{
    public class Database
    {
        private static List<WiiUTitle> DbObject { get; set; }
        private static string TitleKeys => "https://wiiu.titlekeys.com";
        private static string DatabaseFile => "database.json";

        private event EventHandler DownloadTitleCompleted;

        public static void Initialize()
        {
            try
            {
                if (Toolbelt.Database == null)
                    Toolbelt.Database = new Database();

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

        public async void UpdateGame(string titleId, string fullPath)
        {
            var game = FindByTitleId(titleId);

            game.TitleID = game.TitleID.Replace("00050000", "0005000e");

            DownloadTitleCompleted += (outputDir, args) =>
            {
                var outDir = (string) outputDir;
                CleanUpdate(outDir);

                Toolbelt.AppendLog("  - Updating local files...");
                Toolbelt.MoveDirectory(outDir, fullPath);
            };

            await Task.Run(() => DownloadTitle(game));

            Toolbelt.Form1.listBox1.Invoke(new Action(() => { Toolbelt.Form1.listBox1.Enabled = true; }));
        }

        public static WiiUTitle Find(string game_name)
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

        private static WiiUTitle FindByTitleId(string titleId)
        {
            return titleId == null ? new WiiUTitle() : DbObject.Find(t => t.TitleID.ToLower() == titleId.ToLower());
        }

        private static void PopulateTables()
        {
            var json = File.ReadAllText(DatabaseFile);
            DbObject = JsonConvert.DeserializeObject<List<WiiUTitle>>(json);
            DbObject.RemoveAll(t => t.ToString().Contains("()"));
        }

        private static void CleanUpdate(string outputDir)
        {
            Toolbelt.AppendLog("  - Deleting CDecrypt and libeay32...");
            File.Delete(Path.Combine(outputDir, "CDecrypt.exe"));
            File.Delete(Path.Combine(outputDir, "libeay32.dll"));

            Toolbelt.AppendLog("  - Deleting TMD and Ticket...");
            File.Delete(Path.Combine(outputDir, "tmd"));
            File.Delete(Path.Combine(outputDir, "ticket"));
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
                Directory.CreateDirectory(Path.Combine(outputDir, Toolbelt.RemoveInvalidCharacters(wiiUTitle.ToString())));
            outputDir = Path.Combine(outputDir, wiiUTitle.ToString());

            //Download TMD
            Toolbelt.AppendLog("  - Downloading TMD...");
            TMD tmd;
            try
            {
                var tmdFileWithCerts = Network.DownloadData(titleUrl + "tmd");
                tmd = TMD.Load(tmdFileWithCerts);

                //Parse TMD
                Toolbelt.AppendLog("  - Parsing TMD...");
                Toolbelt.AppendLog($"    + Title Version: {tmd.TitleVersion}");
                Toolbelt.AppendLog($"    + {tmd.NumOfContents} Contents");
            }
            catch (Exception ex)
            {
                Toolbelt.AppendLog($"   + Downloading TMD Failed...\n{ex.Message}");
                return string.Empty;
            }

            //Download cetk / ticket
            var ticket = "ticket";
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
                    Toolbelt.AppendLog($"   + Downloading Ticket Failed...\n{ex.Message}");
                    return string.Empty;
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
                return string.Empty;
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
                    Toolbelt.AppendLog(
                        $"  - Downloading Content #{i + 1} of {tmd.NumOfContents} failed...\n{ex.Message}");
                    return string.Empty;
                }
            }

            Toolbelt.AppendLog("  - Decrypting Content...");
            Toolbelt.CDecrypt(outputDir);

            Toolbelt.AppendLog("  - Deleting Encrypted Contents...");
            foreach (var t in tmd.Contents)
                if (File.Exists(Path.Combine(outputDir, t.ContentID.ToString("x8"))))
                    File.Delete(Path.Combine(outputDir, t.ContentID.ToString("x8")));

            DownloadTitleCompleted?.Invoke(outputDir, EventArgs.Empty);

            Toolbelt.AppendLog($"Downloading Title '{wiiUTitle}' v{tmd.TitleVersion} Finished.");
            return outputDir;
        }
    }
}