// Project: MapleSeedU
// File: CemuPath.cs
// Updated By: Jared
// 

using System.IO;
using System.Windows.Forms;

namespace MapleSeedU.Models.Settings
{
    public class CemuPath
    {
        public CemuPath()
        {
            ConfigEntry = new ConfigurationEntry("CemuPath");
        }

        private ConfigurationEntry ConfigEntry { get; }

        public string GetPath()
        {
            if (string.IsNullOrEmpty(ConfigEntry.Value)
                   || !File.Exists(ConfigEntry.Value)) {
                return SetPath();
            }
            return ConfigEntry.Value;
        }

        private string SetPath()
        {
            var diaglog = new OpenFileDialog
            {
                Title = @"Cemu Executable",
                Filter = @"Cemu Executable (cemu.exe) | cemu.exe"
            };

            var result = diaglog.ShowDialog();

            if (result == DialogResult.OK)
                ConfigEntry.Value = Path.GetFullPath(diaglog.FileName);

            return ConfigEntry.Value;
        }

        public void ResetPath()
        {
            ConfigEntry.DeleteKey("CemuPath");
        }
    }
}