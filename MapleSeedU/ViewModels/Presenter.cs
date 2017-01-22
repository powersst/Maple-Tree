// Project: MapleSeedU
// File: Presenter.cs
// Updated By: Jared
// 

#region usings

using System;
using System.IO;
using System.Net;
using System.Windows.Controls;
using MapleSeedU.Models;

#endregion

namespace MapleSeedU.ViewModels
{
    public class Presenter : ViewModelBase
    {
        public static Presenter Instance;
        private string _status = "Github.com/Tsume/Maple-Tree";

        public Presenter()
        {
            if (Instance == null)
                Instance = this;

            UpdateDatabase();

            Status = $"Library Path = {LibraryPath.GetPath()}";
        }

        private void UpdateDatabase()
        {
            if (File.Exists(DbFile) && DateTime.Now <= new FileInfo(DbFile).LastWriteTime.AddDays(1)) return;

            const string url = "https://wiiu.titlekeys.com/json";

            using (var wc = new WebClient())
                wc.DownloadFile(new Uri(url), DbFile);
        }

        public static void WriteLine(string text)
        {
            Instance.LogText += $"[{DateTime.Now:T}] {text} {Environment.NewLine}";
            Instance.RaisePropertyChangedEvent("LogText");
        }
        
        protected override void MyAction()
        {
            TitleInfoViewModel.TitleInfoEntry.PlayTitle();
        }

        public string LogText { get; set; }

        public TitleInfoViewModel TitleInfoViewModel { get; set; } = new TitleInfoViewModel();

        public static CemuPath CemuPath { get; } = new CemuPath();

        public static LibraryPath LibraryPath { get; } = new LibraryPath();

        public static readonly string DbFile = Path.Combine(Path.GetTempPath(), "MapleTree.json");

        public string Status {
            get { return _status; }
            set {
                _status = value;
                RaisePropertyChangedEvent("Status");
            }
        }
    }
}