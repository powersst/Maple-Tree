using MaryJane.Structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MaryJane
{
    public class Database
    {
        public Database()
        {
            WebClient = new WebClient();
        }

        private static WebClient WebClient { get; set; }
        public static List<WiiUTitle> DbObject { get; private set; }
        private static string TitleKeys => "https://wiiu.titlekeys.com";
        private static string CurrentDir => Directory.GetCurrentDirectory();
        private static string DatabaseFile => Path.Combine(CurrentDir, "database.json");

        public async Task Update()
        {
            try
            {
                if (DateTime.Now > new FileInfo(DatabaseFile).CreationTime.AddDays(1))
                {
                    WebClient.DownloadProgressChanged += DownloadProgressChanged;
                    WebClient.DownloadDataCompleted += DownloadDataCompleted;
                    await WebClient.DownloadDataTaskAsync(new Uri(TitleKeys + "/json"));
                }
            }
            catch (Exception e)
            {
                Toolbelt.SetStatus($"{e.Message}\n{e.StackTrace}");
            }
            
            PopulateTables();
            Toolbelt.SetStatus("Database Update to date!");
        }

        private static void PopulateTables()
        {
            var json = File.ReadAllText(DatabaseFile);
            DbObject = JsonConvert.DeserializeObject<List<WiiUTitle>>(json);
            DbObject.RemoveAll(t => t.ToString().Contains("()"));
            DbObject.Remove
        }

        private static void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Toolbelt.Form1.progressBar.Value = e.ProgressPercentage;
            Toolbelt.SetStatus(
                $"Updating Database | {e.ProgressPercentage}% | {Toolbelt.SizeSuffix(e.BytesReceived)} / {Toolbelt.SizeSuffix(e.TotalBytesToReceive)}");
        }

        private void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            File.WriteAllBytes(DatabaseFile, e.Result);
        }
    }
}