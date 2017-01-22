// Project: MapleSeedU
// File: TitleInfoEntry.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using MapleSeedU.ViewModels;
using Newtonsoft.Json;
using TgaLib;

#endregion

namespace MapleSeedU.Models
{
    public class TitleInfoEntry
    {
        public TitleInfoEntry(string fullPath)
        {
            try {
                fullPath = Path.GetDirectoryName(fullPath); //get parent

                if (fullPath != null) {
                    var dir = new DirectoryInfo(fullPath);
                    Root = dir.FullName;
                    Name = dir.Name;

                    SetBootFile();
                    SetTitleID();
                    SetTitleKey();
                }
            }
            catch (Exception) {
                //MessageBox.Show(e.StackTrace);
            }
        }

        public string Name { get; set; }
        public string Root { get; }
        public object BootTex { get; set; }
        private object BootFile { get; set; }
        private string TitleID { get; set; }
        private string TitleKey { get; set; }
        private string Region { get; set; }
        private string Version { get; set; }

        public void PlayTitle()
        {
            try
            {
                var cemuPath = Presenter.CemuPath.GetPath();
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
                Presenter.WriteLine($"Started playing {Name}!");
                process.WaitForExit();
                Presenter.WriteLine($"Stopped playing {Name}!");
            }
            catch (Exception ex)
            {
                Presenter.WriteLine("Error!\r\n" + ex.Message);
            }
        }

        public void SetBootTex()
        {
            var bootTvTexTGA = Path.Combine(Root, "meta//bootTvTex.tga");
            using (var fs = new FileStream(bootTvTexTGA, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new BinaryReader(fs)) {
                var tga = new TgaImage(reader);
                var source = tga.GetBitmap();
                BootTex = source;
            }
        }

        private void SetBootFile()
        {
            var files = Directory.GetFiles(Root, "*.rpx", SearchOption.AllDirectories);
            if (files.Length > 0) {
                BootFile = files[0];
            }
        }

        private void SetTitleID()
        {
            var entries = Directory.GetFiles(Root, "meta.xml", SearchOption.AllDirectories);
            if (entries.Length <= 0) return;

            var xml = new XmlDocument();
            xml.Load(entries[0]);
            var titleIdTag = xml.GetElementsByTagName("title_id");
            var titleVersionTag = xml.GetElementsByTagName("title_version");

            if (titleIdTag.Count > 0)
                TitleID = titleIdTag[0].InnerText;

            if (titleVersionTag.Count > 0)
                Version = titleVersionTag[0].InnerText;
        }

        private void SetTitleKey()
        {
            var json = File.ReadAllText(Presenter.DbFile);
            var list = JsonConvert.DeserializeObject<List<WiiUTitle>>(json);
            var title = list.Find(t => string.Equals(t.TitleID, TitleID, StringComparison.CurrentCultureIgnoreCase));
            TitleKey = title.TitleKey;
        }

        public override string ToString()
        {
            return Helper.RIC($"{Name} {Region} v{Version}");
        }
    }
}