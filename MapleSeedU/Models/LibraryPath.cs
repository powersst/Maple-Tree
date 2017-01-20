// Project: MapleSeedU
// File: LibraryPath.cs
// Updated By: Jared
// 

#region usings

using System.IO;
using System.Windows.Forms;

#endregion

namespace MapleSeedU.Models
{
    public class LibraryPath
    {
        public LibraryPath()
        {
            _libraryPath = new ConfigurationEntry("LibraryPath");
        }

        private ConfigurationEntry _libraryPath { get; }

        public string GetPath()
        {
            return !string.IsNullOrEmpty(_libraryPath.Value) ? _libraryPath.Value : SetPath();
        }

        private string SetPath()
        {
            var diaglog = new FolderBrowserDialog {Description = @"Cemu Library Path (Root folder of Wii U Games)"};
            var result = diaglog.ShowDialog();

            if (result == DialogResult.OK)
                _libraryPath.Value = Path.GetFullPath(diaglog.SelectedPath);

            return _libraryPath.Value;
        }
    }
}