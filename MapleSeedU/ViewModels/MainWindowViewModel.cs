// Project: MapleSeedU
// File: Presenter.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
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

        private TitleInfoEntry _titleInfoEntry;

        public MainWindowViewModel()
        {
            var dispatcherTimer = new DispatcherTimer(TimeSpan.Zero,
                DispatcherPriority.ApplicationIdle, OnLoadComplete,
                Application.Current.Dispatcher);

            if (Instance == null)
                Instance = this;

            UpdateDatabase();

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

        private LibraryPath LibraryPath => new LibraryPath();

        public List<TitleInfoEntry> TitleInfoEntries => TitleInfoEntry.Entries;

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
        public ICommand CacheUpdateCommand => new CommandHandler(async () => await ThemeUpdate());

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

        private void LockControls()
        {
            CacheUpdateEnabled = false;
            RaisePropertyChangedEvent("CacheUpdateEnabled");
        }

        private void UnlockControls()
        {
            CacheUpdateEnabled = true;
            RaisePropertyChangedEvent("CacheUpdateEnabled");
            RaisePropertyChangedEvent("TitleInfoEntries");
        }

        private static async void OnLoadComplete(object sender, EventArgs e)
        {
            (sender as DispatcherTimer)?.Stop();
            await Instance.LoadCache();
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
            LockControls();

            await Task.Run(() => {
                var files = Directory.GetFiles(Path.Combine(Path.GetTempPath(), "MapleTree"));
                ProgressBarMax = files.Length;

                var list = new TitleInfoEntry[ProgressBarMax = files.Length];

                for (var i = 0; i < files.Length; i++)
                    try {
                        var file = files[i];
                        list[i] = TitleInfoEntry.LoadCache(file, true);
                        ProgressBarCurrent++;
                    }
                    catch (Exception e) {
                        WriteLine(e.StackTrace);
                    }

                TitleInfoEntry.Entries = new List<TitleInfoEntry>(list);
                TitleInfoEntry.Entries.Sort((t1, t2) => string.Compare(t1.Name, t2.Name, StringComparison.Ordinal));
            });

            UnlockControls();
        }

        private async Task ThemeUpdate()
        {
            ProgressBarCurrent = 0;
            LockControls();

            await Task.Run(() => {
                var files = Directory.GetFiles(LibraryPath.GetPath(), "*.rpx", SearchOption.AllDirectories);

                var list = new TitleInfoEntry[ProgressBarMax = files.Length];

                for (var i = 0; i < files.Length; i++) {
                    var file = files[i];
                    list[i] = TitleInfoEntry.LoadCache(file, false);
                    ProgressBarCurrent++;
                }

                TitleInfoEntry.Entries = new List<TitleInfoEntry>(list);
            });

            TitleInfoEntry = TitleInfoEntry.Entries[0];
            UnlockControls();
        }
    }
}