// Project: MapleSeedU
// File: TitleInfoEntry.cs
// Updated By: Jared
// 

#pragma warning disable IDE1006 // Naming Styles

#region usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Xml;
using MapleSeedU.ViewModels;
using Newtonsoft.Json;
using TgaLib;

#endregion

namespace MapleSeedU.Models
{
    [Serializable]
    public class TitleInfoEntry
    {
        private TitleInfoEntry(string fullPath)
        {
            if (Entries == null)
                Entries = new List<TitleInfoEntry>();

            Raw = fullPath;

            var folder = Path.GetDirectoryName(BootFile = fullPath);
            Root = Path.GetDirectoryName(folder); //get parent

            if (Root == null) return;
            Name = new DirectoryInfo(Root).Name;

            SetTitleID();
            SetTitleKey();
        }

        public static List<TitleInfoEntry> Entries { get; set; }

        public byte[] BootTex { get; private set; }
        public string Raw { get; }
        public string Name { get; }
        private string Root { get; }
        private string BootFile { get; }
        private string TitleID { get; set; }
        private string TitleKey { get; set; }
        private string Region { get; set; }
        private string Version { get; set; }
        private Color CachedColor { get; set; }
        internal bool IsCached { private get; set; }
        private static string CacheLocation => Path.GetFullPath(Path.Combine(Path.GetTempPath(), "MapleTree"));

        public static TitleInfoEntry LoadCache(string path, bool isCached)
        {
            if (!isCached) {
                var entry = new TitleInfoEntry(path);
                entry.CacheTheme();
                return entry;
            }

            if (!Directory.Exists(Path.GetDirectoryName(path))) {
                var dir = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(dir)) return null;
                Directory.CreateDirectory(dir);
            }

            if (!File.Exists(path) || new FileInfo(path).Length == 0) return null;
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                IFormatter formatter = new BinaryFormatter();
                var tie = (TitleInfoEntry) formatter.Deserialize(stream);
                return tie;
            }
        }

        private void Save()
        {
            if (!Directory.Exists(CacheLocation)) Directory.CreateDirectory(CacheLocation);
            using (
                Stream stream = new FileStream(Path.Combine(CacheLocation, TitleID), FileMode.Create, FileAccess.Write,
                    FileShare.Read)) {
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

            ThemeManagerHelper.CreateAppStyleBy(System.Windows.Media.Color.FromArgb(CachedColor.A, CachedColor.R,
                CachedColor.G, CachedColor.B));

            MainWindowViewModel.Instance.Status = Root;
        }

        public void CacheTheme(bool force = false)
        {
            if (force) IsCached = false;
            if (IsCached) return;

            SetBootTex();

            var bmp = ImageAnalysis.FromBytes(BootTex);

            if (CachedColor.IsEmpty || force)
                CachedColor = ImageAnalysis.GetRandomColour(bmp);

            Save();

            IsCached = true;
            MainWindowViewModel.Instance.Status = $"Loading theme, {Name}";
        }

        public override string ToString()
        {
            return Helper.RIC($"{Name} {Region} [v{Version}]");
        }
    }
}