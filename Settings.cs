using System.IO;
using System.Windows.Forms;
using IniParser;
using MaryJane.Properties;

namespace MaryJane
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
                var fbd = new FolderBrowserDialog();
                var result = fbd.ShowDialog();

                if (string.IsNullOrWhiteSpace(fbd.SelectedPath) && result == DialogResult.OK)
                    return value;
                value = fbd.SelectedPath;

                WriteKeyValue("TitleDirectory", value);
                return value;
            }
        }

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