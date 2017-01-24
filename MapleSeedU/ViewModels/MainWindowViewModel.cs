// Project: MapleSeedU
// File: Presenter.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MapleSeedU.Models;

#endregion

namespace MapleSeedU.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static MainWindowViewModel Instance;

        public readonly string DbFile = Path.Combine(Path.GetTempPath(), "MapleTree.json");

        private string _status = "Github.com/Tsume/Maple-Tree";
        private CollectionView _titleInfoEntries;

        private TitleInfoEntry _titleInfoEntry;

        public MainWindowViewModel()
        {
            if (Instance == null)
                Instance = this;

            UpdateDatabase();

            Status = $"Library Path = {LibraryPath.GetPath()}";
        }

        public string LogText { get; set; }
        
        public CemuPath CemuPath => new CemuPath();

        public LibraryPath LibraryPath => new LibraryPath();

        public CollectionView TitleInfoEntries {
            get { return _titleInfoEntries; }
            set {
                _titleInfoEntries = value;
                RaisePropertyChangedEvent("TitleInfoEntries");
            }
        }

        public TitleInfoEntry TitleInfoEntry {
            get { return _titleInfoEntry; }
            set {
                if (_titleInfoEntry == value) return;
                _titleInfoEntry = value;
                value.UpdateTheme();
                RaisePropertyChangedEvent("TitleInfoEntry");
                RaisePropertyChangedEvent("BackgroundImage");
            }
        }

        public BitmapSource BackgroundImage => TitleInfoEntry?.BootTex;

        public string Status {
            get { return _status; }
            set {
                _status = value;
                RaisePropertyChangedEvent("Status");
            }
        }

        public ICommand PlayTitleCommand => new CommandHandler(PlayTitle);
        public ICommand CacheUpdateCommand => new CommandHandler(CacheUpdate);

        public bool CacheUpdateEnabled { get; set; } = true;

        private void UpdateDatabase()
        {
            if (File.Exists(DbFile) && DateTime.Now <= new FileInfo(DbFile).LastWriteTime.AddDays(1)) return;

            const string url = "https://wiiu.titlekeys.com/json";

            using (var wc = new WebClient()) {
                wc.DownloadFile(new Uri(url), DbFile);
            }
        }

        public static void WriteLine(string text)
        {
            Instance.LogText += $"[{DateTime.Now:T}] {text} {Environment.NewLine}";
            Instance.RaisePropertyChangedEvent("LogText");
        }

        private void PlayTitle()
        {
            TitleInfoEntry.PlayTitle();
        }

        private void CacheUpdate()
        {
            CacheUpdateEnabled = false;
            RaisePropertyChangedEvent("CacheUpdateEnabled");

            var path = LibraryPath.GetPath();
            var files = Directory.EnumerateFiles(path, "*.rpx", SearchOption.AllDirectories);

            var list = new List<TitleInfoEntry>();

            foreach (var file in files) {
                var entry = new TitleInfoEntry(file);
                entry.CacheTheme();
                list.Add(entry);
            }

            TitleInfoEntries = new CollectionView(list);

            CacheUpdateEnabled = true;
            RaisePropertyChangedEvent("CacheUpdateEnabled");
        }
    }
}