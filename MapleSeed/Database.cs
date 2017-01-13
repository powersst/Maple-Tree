using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using libWiiSharp;
using MapleSeed;
using MaryJane.Structs;
using Newtonsoft.Json;

namespace MaryJane
{
    public class Database
    {
        private static string TitleKeys => "https://wiiu.titlekeys.com";
        private static string DatabaseFile => "database.json";
        private static List<WiiUTitle> DbObject { get; set; } = new List<WiiUTitle>();

        public static async void Initialize()
        {
            try
            {
                if (Toolbelt.Database == null)
                    Toolbelt.Database = new Database();

                if (Toolbelt.Settings == null)
                    Toolbelt.Settings = new Settings();

                if (DateTime.Now > new FileInfo(DatabaseFile).LastWriteTime.AddDays(1))
                {
                    var data = await Network.DownloadData(TitleKeys + "/json");

                    if (File.Exists(DatabaseFile))
                        File.Delete(DatabaseFile);
                    File.WriteAllBytes(DatabaseFile, data);
                }
            }
            catch (Exception e)
            {
                Toolbelt.AppendLog($"{e.Message}\n{e.StackTrace}");
            }

            var json = File.ReadAllText(DatabaseFile);
            DbObject = JsonConvert.DeserializeObject<List<WiiUTitle>>(json);
            DbObject.RemoveAll(t => t.ToString().Contains("()"));
            Toolbelt.AppendLog("Database Update to date!");
        }

        public void updateGame(string titleId, string fullPath)
        {
            titleId = titleId.Replace("00050000", "0005000e");
            UpdateGame(titleId, fullPath);
        }

        public void UpdateGame(string titleId, string fullPath)
        {
            var game = FindByTitleId(titleId);

            if (Toolbelt.Form1 != null)
            {
                if (Toolbelt.Form1.fullTitle.Checked)
                {
                    game.TitleID = game.TitleID.Replace("00050000", "0005000e");
                }
            }

            Toolbelt.SetStatus($"Updating {titleId}");

            DownloadTitle(game, fullPath);

            if (Toolbelt.Form1 != null)
                Toolbelt.Form1.listBox1.Enabled = true;

            Toolbelt.SetStatus(string.Empty);
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

        private static void CleanUpdate(string outputDir, TMD tmd)
        {
            try
            {
                Toolbelt.AppendLog("  - Deleting Encrypted Contents...");
                foreach (var t in tmd.Contents)
                    if (File.Exists(Path.Combine(outputDir, t.ContentID.ToString("x8"))))
                        File.Delete(Path.Combine(outputDir, t.ContentID.ToString("x8")));

                Toolbelt.AppendLog("  - Deleting CDecrypt and libeay32...");
                File.Delete(Path.Combine(outputDir, "CDecrypt.exe"));
                File.Delete(Path.Combine(outputDir, "libeay32.dll"));
            }
            catch
            {
            }
        }

        private static async Task<int> TicketHandled(WiiUTitle wiiUTitle, string outputDir, string titleUrl)
        {
            var buffer = new byte[0];
            try
            {
                Toolbelt.AppendLog("  - Downloading Ticket...");
                buffer = await Network.DownloadData(Path.Combine(titleUrl, "cetk"));
            }
            catch (Exception ex)
            {
                try
                {
                    if (wiiUTitle.Ticket == "1")
                    {
                        var WII_TIK_URL = "https://wiiu.titlekeys.com/ticket/";
                        var cetkUrl = $"{WII_TIK_URL}{wiiUTitle.TitleID.ToLower()}.tik";
                        buffer = await Network.DownloadData(cetkUrl);
                    }
                }
                catch
                {
                    Toolbelt.AppendLog($"   + Downloading Ticket Failed...\n{ex.Message}");
                    return 0;
                }
            }
            File.WriteAllBytes(Path.Combine(outputDir, "cetk"), buffer);

            // Parse Ticket
            if (buffer.Length > 0)
            {
                Toolbelt.AppendLog("   + Parsing Ticket...");
                Ticket.Load(buffer);
            }
            else
            {
                Toolbelt.AppendLog("   + Ticket Unavailable...");
                return 0;
            }

            return 1;
        }

        private static async Task<TMD> HandleTmd(string outputDir, string titleUrl)
        {
            try
            {
                var tmdFile = Path.Combine(outputDir, "tmd");

                if (!File.Exists(tmdFile))
                {
                    Toolbelt.AppendLog("  - Downloading TMD...");
                    var buffer = await Network.DownloadData(titleUrl + "tmd");

                    Toolbelt.AppendLog("  - Saving TMD...");
                    File.WriteAllBytes(Path.Combine(outputDir, "tmd"), buffer);
                }

                Toolbelt.AppendLog("  - Loading TMD...");
                var tmd = TMD.Load(tmdFile);

                Toolbelt.AppendLog("  - Parsing TMD...");
                Toolbelt.AppendLog($"    + Title Version: {tmd.TitleVersion}");
                Toolbelt.AppendLog($"    + {tmd.NumOfContents} Contents");

                return tmd;
            }
            catch (Exception ex)
            {
                Toolbelt.AppendLog($"   + Downloading TMD Failed...\n{ex.Message}");
            }

            return null;
        }

        private static async Task<int> ContentHandled(TMD tmd, string outputDir, string titleUrl)
        {
            for (var i = 0; i < tmd.NumOfContents; i++)
            {
                var size = Toolbelt.SizeSuffix((long) tmd.Contents[i].Size);
                Toolbelt.AppendLog($"  - Downloading Content #{i + 1} of {tmd.NumOfContents}... ({size} bytes)");

                var contentPath = Path.Combine(outputDir, tmd.Contents[i].ContentID.ToString("x8"));
                if (Toolbelt.IsValid(tmd.Contents[i], contentPath))
                    Toolbelt.AppendLog("   + Using Local File, Skipping...");
                else
                    try
                    {
                        var downloadUrl = titleUrl + tmd.Contents[i].ContentID.ToString("x8");
                        var outputdir = Path.Combine(outputDir, tmd.Contents[i].ContentID.ToString("x8"));
                        await Network.DownloadFile(downloadUrl, outputdir);
                    }
                    catch (Exception ex)
                    {
                        Toolbelt.AppendLog(
                            $"  - Downloading Content #{i + 1} of {tmd.NumOfContents} failed...\n{ex.Message}");
                        return 0;
                    }
            }

            return 1;
        }

        private async void DownloadTitle(WiiUTitle wiiUTitle, string fullPath)
        {
            var outputDir = Path.GetFullPath(fullPath);

            if (fullPath.EndsWith(".rpx"))
            {
                var folder = Path.GetDirectoryName(fullPath);
                if (folder.EndsWith("code"))
                {
                    outputDir = Path.GetDirectoryName(folder);
                }
            }

            Toolbelt.AppendLog($"Output Directory '{outputDir}'");

            Toolbelt.AppendLog($"Downloading Title {wiiUTitle.TitleID} v[Latest]...");

            const string wiiNusUrl = "http://nus.cdn.shop.wii.com/ccs/download/";
            const string wiiWupUrl = "http://ccs.cdn.wup.shop.nintendo.net/ccs/download/";
            string titleUrl = $"{wiiWupUrl}{wiiUTitle.TitleID}/";
            string titleUrl2 = $"{wiiNusUrl}{wiiUTitle.TitleID}/";
            
            //Download Ticket
            if (await TicketHandled(wiiUTitle, outputDir, titleUrl) == 0) return;

            //Download TMD
            var tmd = await HandleTmd(outputDir, titleUrl);

            //Download Content
            if (await ContentHandled(tmd, outputDir, titleUrl) == 0) return;

            Toolbelt.AppendLog("  - Decrypting Content...");
            Toolbelt.CDecrypt(outputDir);

            CleanUpdate(outputDir, tmd);

            Toolbelt.Form1?.listBox1.Invoke(new Action(() => { Toolbelt.Form1.listBox1.Enabled = true; }));
            Toolbelt.AppendLog($"Downloading Title '{wiiUTitle}' v{tmd.TitleVersion} Finished.");
        }
    }
}