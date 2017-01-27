// Project: MapleSeedU
// File: LibraryPath.cs
// Updated By: Jared
// 

#region usings

using System.IO;
using System.Windows.Forms;

#endregion

namespace MapleSeedU.Models.Settings
{
    public class LibraryPath
    {
        public LibraryPath()
        {
            ConfigEntry = new ConfigurationEntry("LibraryPath");
        }

        private ConfigurationEntry ConfigEntry { get; }

        public string GetPath()
        {
            if (string.IsNullOrEmpty(ConfigEntry.Value) 
                || !Directory.Exists(ConfigEntry.Value)) {
                return SetPath();
            }
            return ConfigEntry.Value;
        }

        private string SetPath()
        {
            var diaglog = new FolderBrowserDialog {Description = @"Cemu Library Path (Root folder of Wii U Games)"};
            var result = diaglog.ShowDialog();

            if (result == DialogResult.OK)
                ConfigEntry.Value = Path.GetFullPath(diaglog.SelectedPath);

            return ConfigEntry.Value;
        }

        public void ResetPath()
        {
            ConfigEntry.DeleteKey("LibraryPath");
        }
    }
}