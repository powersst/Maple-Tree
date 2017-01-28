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
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MapleSeedU.Models;
using MapleSeedU.Models.Settings;

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
        }

        private void Reset()
        {
            CemuPath.ResetPath();
            LibraryPath.ResetPath();
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
                value?.UpdateTheme();
                RaisePropertyChangedEvent("TitleInfoEntry");
                RaisePropertyChangedEvent("BackgroundImage");
            }
        }

        public BitmapSource BackgroundImage => ImageAnalysis.FromBytes(TitleInfoEntry?.BootTex);

        public ICommand ResetCommand => new CommandHandler(Reset);

        public ICommand PlayTitleCommand => new CommandHandler(PlayTitle);

        public ICommand CacheUpdateCommand => new CommandHandler(async () => await ThemeUpdate(true));

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
            
            await Instance.ThemeUpdate();
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

            string tempPath;
            if (!Directory.Exists(tempPath = Path.Combine(Path.GetTempPath(), "MapleTree")))
                Directory.CreateDirectory(tempPath);

            await Task.Run(() => {
                var files = Directory.GetFiles(tempPath);
                ProgressBarMax = files.Length;

                var list = new TitleInfoEntry[ProgressBarMax = files.Length];

                for (var i = 0; i < files.Length; i++)
                    try {
                        var file = files[i];
                        list[i] = TitleInfoEntry.LoadCache(file, true);
                        list[i].IsCached = true;
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

        private async Task ThemeUpdate(bool forceUpdate = false)
        {
            await Instance.LoadCache();

            ProgressBarCurrent = 0;
            LockControls();

            var files = Directory.GetFiles(LibraryPath.GetPath(), "*.rpx", SearchOption.AllDirectories);

            await Task.Run(() => {

                var list = new TitleInfoEntry[ProgressBarMax = files.Length];

                for (var i = 0; i < files.Length; i++) {
                    var file = files[i];

                    if (TitleInfoEntry.Entries == null) continue;
                    var entries = TitleInfoEntry.Entries.FindAll(entry => entry.Raw == file).Count;
                    if (entries > 0) continue;

                    list[i] = TitleInfoEntry.LoadCache(file, false);
                    TitleInfoEntry.Entries?.Add(list[i]);

                    ProgressBarCurrent++;
                }

                ProgressBarCurrent = 0;
                if (TitleInfoEntry.Entries == null) return;
                foreach (var t in TitleInfoEntry.Entries) {
                    if (forceUpdate) t.CacheTheme(true);
                    ProgressBarCurrent++;
                }
            });

            TitleInfoEntry = TitleInfoEntry.Entries[0];
            UnlockControls();

            Status = $"Library Path = {LibraryPath.GetPath()}";
        }
    }
}