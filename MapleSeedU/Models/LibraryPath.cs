// Project: MapleSeedU
// File: LibraryPath.cs
// Updated By: Jared
// 

using System.IO;
using System.Windows.Forms;

namespace MapleSeedU.Models
{
    public class LibraryPath
    {
        private ConfigurationEntry _libraryPath { get; set; }

        public LibraryPath()
        {
            _libraryPath = new ConfigurationEntry("LibraryPath");
        }

        public string GetPath()
        {
            var value = _libraryPath.Value;
            if (!string.IsNullOrEmpty(value)) return string.Empty;

            var diaglog = new FolderBrowserDialog {Description = @"Cemu Library Path (Root folder of Wii U Games)"};
            var result = diaglog.ShowDialog();

            if (result == DialogResult.OK) {
                _libraryPath.Value = Path.GetFullPath(diaglog.SelectedPath);
            }

            return string.Empty;
        }
    }
}