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
            _libraryPath = new ConfigurationEntry("CemuPath");
        }

        private ConfigurationEntry _libraryPath { get; }

        public string GetPath()
        {
            return !string.IsNullOrEmpty(_libraryPath.Value) ? _libraryPath.Value : SetPath();
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
                _libraryPath.Value = Path.GetFullPath(diaglog.FileName);

            return _libraryPath.Value;
        }

        public void ResetPath()
        {
            _libraryPath.DeleteKey("CemuPath");
        }
    }
}