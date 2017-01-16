// Project: MapleSeed
// File: Settings.cs
// Updated By: Jared
// 

#region usings

using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using IniParser;
using MapleRoot;
using MapleRoot.Common;
using MapleSeed.Properties;

#endregion

namespace MapleSeed
{
    public class Settings
    {
        public Settings()
        {
            if (!File.Exists(ConfigFile) || new FileInfo(ConfigFile).Length <= 0)
                File.WriteAllText(ConfigFile, Resources.Settings_DefaultSettings);
        }

        public string TitleDirectory {
            get {
                var value = GetKeyValue("TitleDirectory");

                if (!string.IsNullOrEmpty(value)) return value;
                var fbd = new FolderBrowserDialog {Description = @"Please select your Cemu Game Directory"};
                var result = fbd.ShowDialog();

                if (string.IsNullOrWhiteSpace(fbd.SelectedPath) || result != DialogResult.OK)
                    Application.Exit();

                value = fbd.SelectedPath;
                WriteKeyValue("TitleDirectory", value);
                return value;
            }
        }

        public string CemuDirectory {
            get {
                var value = GetKeyValue("CemuDirectory");

                if (!string.IsNullOrEmpty(value)) return value;
                var ofd = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Filter = @"Cemu Excutable |cemu.exe"
                };
                DialogResult result = DialogResult.Cancel;
                Toolbelt.Form1.Invoke(new Action(()=> result = ofd.ShowDialog()));

                if (string.IsNullOrWhiteSpace(ofd.FileName) || result != DialogResult.OK)
                    Application.Exit();
                
                value = Path.GetDirectoryName(ofd.FileName);
                WriteKeyValue("CemuDirectory", value);
                return value;
            }
        }

        public string Username {
            get { return GetKeyValue("Username"); }

            set { WriteKeyValue("Username", value); }
        }

        public string Hub
        {
            get {
                var value = GetKeyValue("Hub");
                if (string.IsNullOrEmpty(value))
                    WriteKeyValue("Hub", value = "mapletree.tsumes.com");
                return value;
            }
        }

        public string Serial {
            get {
                var value = GetKeyValue("Serial");
                if (string.IsNullOrEmpty(value)) WriteKeyValue("Serial", Toolkit.UniqueID());
                return value;
            }
        }

        public bool FullScreenMode {
            get {
                var value = GetKeyValue("FullScreenMode");
                if (string.IsNullOrEmpty(value)) WriteKeyValue("FullScreenMode", false.ToString());
                return value == "True";
            }

            set { WriteKeyValue("FullScreenMode", value.ToString()); }
        }

        public static Settings Instance => Toolbelt.Settings;
        private static string ConfigFile => "configuration.ini";
        private static string ConfigName => "MapleTree";

        private string GetKeyValue(string key)
        {
            var parser = new FileIniDataParser();
            var data = parser.ReadFile(ConfigFile);
            return data[ConfigName][key] ?? "";
        }

        private void WriteKeyValue(string key, string value)
        {
            var parser = new FileIniDataParser();
            var data = parser.ReadFile(ConfigFile);
            data[ConfigName][key] = value;
            parser.WriteFile(ConfigFile, data);
        }
    }
}