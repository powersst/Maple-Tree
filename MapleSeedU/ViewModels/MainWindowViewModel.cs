// Project: MapleSeedU
// File: Presenter.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
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
            new DispatcherTimer(TimeSpan.Zero, DispatcherPriority.ApplicationIdle, dispatcherTimer_Tick,
                Application.Current.Dispatcher);

            if (Instance == null)
                Instance = this;

            UpdateDatabase();

            Instance.LoadCache();

            Status = $"Library Path = {LibraryPath.GetPath()}";
        }

        public string Status {
            get { return _status; }
            set {
                _status = value;
                RaisePropertyChangedEvent("Status");
            }
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

        public BitmapSource BackgroundImage => ImageAnalysis.FromBytes(TitleInfoEntry?.BootTex);

        public ICommand PlayTitleCommand => new CommandHandler(PlayTitle);
        public ICommand CacheUpdateCommand => new CommandHandler(async () => await ThemeUpdate(true), CacheUpdateEnabled);

        public bool CacheUpdateEnabled { get; set; } = true;

        private int _progressBarMax { get; set; } = 100;

        public int ProgressBarMax {
            get { return _progressBarMax; }
            set {
                _progressBarMax = value;
                RaisePropertyChangedEvent("ProgressBarMax");
            }
        }

        private int _progressBarCurrent { get; set; } = 100;

        public int ProgressBarCurrent {
            get { return _progressBarCurrent; }
            set {
                _progressBarCurrent = value;
                RaisePropertyChangedEvent("ProgressBarCurrent");
            }
        }

        private static async void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Instance.TitleInfoEntries == null || Instance.TitleInfoEntries.Count == 0)
                await Instance.ThemeUpdate(true);

            (sender as DispatcherTimer)?.Stop();
        }

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

        private async Task LoadCache()
        {
            ProgressBarCurrent = 0;
            CacheUpdateEnabled = false;
            RaisePropertyChangedEvent("CacheUpdateEnabled");

            var list = new List<TitleInfoEntry>();

            await Task.Run(() => {
                var files = Directory.GetFiles(Path.Combine(Path.GetTempPath(), "MapleTree"));
                ProgressBarMax = files.Length;

                foreach (var file in files) {
                    list.Add(TitleInfoEntry.LoadCache(file));
                    ProgressBarCurrent++;
                }
                list.Sort((t1, t2) => string.Compare(t1.Name, t2.Name, StringComparison.Ordinal));
            });

            TitleInfoEntries = new CollectionView(list);

            CacheUpdateEnabled = true;
            RaisePropertyChangedEvent("CacheUpdateEnabled");
        }

        private async Task ThemeUpdate(bool forceUpdate = false)
        {
            ProgressBarCurrent = 0;
            CacheUpdateEnabled = false;
            RaisePropertyChangedEvent("CacheUpdateEnabled");

            var list = new List<TitleInfoEntry>();

            await Task.Run(() => {
                var files = Directory.GetFiles(LibraryPath.GetPath(), "*.rpx", SearchOption.AllDirectories);
                ProgressBarMax = files.Length;

                foreach (var file in files) {
                    var entry = new TitleInfoEntry(file);

                    if (forceUpdate) {
                        entry.CacheTheme();
                        list.Add(entry);
                    }
                    else {
                        list.Add(entry);
                    }
                    ProgressBarCurrent++;
                }
            });

            TitleInfoEntries = new CollectionView(list);

            CacheUpdateEnabled = true;
            RaisePropertyChangedEvent("CacheUpdateEnabled");
        }
    }
}