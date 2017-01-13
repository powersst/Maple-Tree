using System.IO;
using System.Windows.Forms;
using IniParser;
using MapleSeed.Properties;

namespace MapleSeed
{
    public class Settings
    {
        public Settings()
        {
            if (!File.Exists(ConfigFile))
                File.WriteAllText(ConfigFile, Resources.Settings_DefaultSettings);
        }

        public string TitleDirectory
        {
            get
            {
                var value = GetKeyValue("TitleDirectory");

                if (!string.IsNullOrEmpty(value)) return value;
                var fbd = new FolderBrowserDialog { Description = "Please select your Cemu Game Directory" };
                var result = fbd.ShowDialog();

                if (string.IsNullOrWhiteSpace(fbd.SelectedPath) && result == DialogResult.OK)
                    return value;
                value = fbd.SelectedPath;

                WriteKeyValue("TitleDirectory", value);
                return value;
            }
        }

        public string CemuDirectory
        {
            get
            {
                var value = GetKeyValue("CemuDirectory");

                if (!string.IsNullOrEmpty(value)) return value;
                var ofd = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Filter = "Cemu Excutable |cemu.exe"
                };
                var result = ofd.ShowDialog();

                if (string.IsNullOrWhiteSpace(ofd.FileName) && result == DialogResult.OK)
                    return value;

                value = Path.GetDirectoryName(ofd.FileName);
                WriteKeyValue("CemuDirectory", value);
                return value;
            }
        }

        public string Username
        {
            get
            {
                return GetKeyValue("Username");
            }

            set
            {
                WriteKeyValue("Username", value);
            }
        }

        public bool FullScreenMode
        {
            get
            {
                return bool.Parse(GetKeyValue("FullScreenMode")); ;
            }

            set
            {
                WriteKeyValue("FullScreenMode", value.ToString());
            }
        }

        public static Settings Instance => Toolbelt.Settings;
        private static string ConfigFile => "configuration.ini";
        private static string ConfigName => "MaryJane";

        private string GetKeyValue(string key)
        {
            var parser = new FileIniDataParser();
            var data = parser.ReadFile(ConfigFile);
            return data[ConfigName][key];
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