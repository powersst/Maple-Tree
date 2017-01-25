// Project: MapleSeedU
// File: TitleInfoEntry.cs
// Updated By: Jared
// 
#pragma warning disable IDE1006 // Naming Styles

#region usings

using MapleSeedU.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using TgaLib;

#endregion

namespace MapleSeedU.Models
{
    [Serializable]
    public class TitleInfoEntry
    {
        public TitleInfoEntry(string fullPath)
        {
            Raw = fullPath;

            var folder = Path.GetDirectoryName(BootFile = fullPath);
            Root = Path.GetDirectoryName(folder); //get parent

            if (Root == null) return;
            Name = new DirectoryInfo(Root).Name;

            SetTitleID();
            SetTitleKey();
        }
        
        public byte[] BootTex { get; private set; }
        private string Raw { get; }
        public string Name { get; }
        private string Root { get; }
        private string BootFile { get; }
        private string TitleID { get; set; }
        private string TitleKey { get; set; }
        private string Region { get; set; }
        private string Version { get; set; }
        private System.Drawing.Color CachedColor { get; set; }
        private string CacheLocation => Path.GetFullPath(Path.Combine(Path.GetTempPath(), "MapleTree"));

        public static TitleInfoEntry LoadCache(string fullpath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(fullpath))) {
                var path = Path.GetDirectoryName(fullpath);
                if (string.IsNullOrEmpty(path)) return null;
                Directory.CreateDirectory(path);
            }

            using (Stream stream = new FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                IFormatter formatter = new BinaryFormatter();
                var tie = (TitleInfoEntry)formatter.Deserialize(stream);
                return tie;
            }
        }

        public TitleInfoEntry Load()
        {
            if (!Directory.Exists(CacheLocation)) Directory.CreateDirectory(CacheLocation);
            if (!File.Exists(Path.Combine(CacheLocation, TitleID))) return null;

            using (Stream stream = new FileStream(Path.Combine(CacheLocation, TitleID), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                IFormatter formatter = new BinaryFormatter();
                var tie = (TitleInfoEntry)formatter.Deserialize(stream);
                return tie;
            }
        }

        private void Save()
        {
            if (!Directory.Exists(CacheLocation)) Directory.CreateDirectory(CacheLocation);
            using (Stream stream = new FileStream(Path.Combine(CacheLocation, TitleID), FileMode.Create, FileAccess.Write, FileShare.Read)) {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
            }
        }

        public async void PlayTitle()
        {
            try {
                var cemuPath = MainWindowViewModel.Instance.CemuPath.GetPath();
                var workingDir = Path.GetDirectoryName(cemuPath);

                var o1 = true ? "-f" : "";
                if (workingDir == null) return;

                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = cemuPath,
                        Arguments = $"{o1} -g \"{BootFile}\"",
                        WorkingDirectory = workingDir,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }
                };

                process.Start();
                MainWindowViewModel.WriteLine($"Started playing {Name}!");
                await Task.Run(() => {
                    process.WaitForExit();
                    MainWindowViewModel.WriteLine($"Stopped playing {Name}!");
                });
            }
            catch (Exception ex) {
                MainWindowViewModel.WriteLine("Error!\r\n" + ex.Message);
            }
        }

        private void SetTitleID()
        {
            var metaxml = Path.Combine(Root, "meta", "meta.xml");
            if (!File.Exists(metaxml)) return;

            var xml = new XmlDocument();
            xml.Load(metaxml);
            var titleIdTag = xml.GetElementsByTagName("title_id");
            var titleVersionTag = xml.GetElementsByTagName("title_version");

            if (titleIdTag.Count > 0)
                TitleID = titleIdTag[0].InnerText;

            if (titleVersionTag.Count > 0)
                Version = titleVersionTag[0].InnerText;
        }

        private void SetTitleKey()
        {
            var json = File.ReadAllText(MainWindowViewModel.Instance.DbFile);
            var list = JsonConvert.DeserializeObject<List<WiiUTitle>>(json);
            var title = list.Find(t => string.Equals(t.TitleID, TitleID, StringComparison.CurrentCultureIgnoreCase));
            TitleKey = title.TitleKey;
        }

        private string GetShaderHash()
        {
            if (new FileInfo(BootFile).Extension != ".rpx") return string.Empty;
            var data = File.ReadAllBytes(BootFile);
            return Helper.generateHashFromRawRPXData(data, data.Length).ToString("x8");
        }

        private void SetBootTex()
        {
            if (BootTex == null) {
                var bootTvTexTGA = Path.Combine(Root, "meta//bootTvTex.tga");
                if (!File.Exists(bootTvTexTGA)) return;

                using (var fs = new FileStream(bootTvTexTGA, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var reader = new BinaryReader(fs)) {
                    var tga = new TgaImage(reader);
                    BootTex = tga.GetBitmap().ToBytes();
                }
            }
        }

        public void UpdateTheme()
        {
            CacheTheme();

            ThemeManagerHelper.CreateAppStyleBy(Color.FromArgb(CachedColor.A, CachedColor.R, CachedColor.G, CachedColor.B));
            
            MainWindowViewModel.Instance.Status = Root;
        }

        public void CacheTheme()
        {
            SetBootTex();

            var bmp = ImageAnalysis.FromBytes(BootTex);

            if (CachedColor.IsEmpty)
                CachedColor = ImageAnalysis.GetRandomColour(bmp);
            
            Save();

            MainWindowViewModel.WriteLine(MainWindowViewModel.Instance.Status = $"Generating theme for {Name}, complete!");
        }

        public override string ToString()
        {
            return Helper.RIC($"{Name} {Region} [v{Version}]");
        }
    }
}