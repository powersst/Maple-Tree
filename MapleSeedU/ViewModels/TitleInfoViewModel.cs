// Project: MapleSeedU
// File: TitleInfoViewModel.cs
// Updated By: Jared
// 

#region usings

using MapleSeedU.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

#endregion

namespace MapleSeedU.ViewModels
{
    public class TitleInfoViewModel : INotifyPropertyChanged
    {
        public CollectionView TitleInfoEntries { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void CacheUpdate()
        {
            var path = MainWindowViewModel.Instance.LibraryPath.GetPath();
            var files = Directory.EnumerateFiles(path, "*.rpx", SearchOption.AllDirectories);

            var list = new List<TitleInfoEntry>();

            foreach (var file in files) {
                var entry = new TitleInfoEntry(file);
                entry.CacheTheme();
                list.Add(entry);
            }

            TitleInfoEntries = new CollectionView(list);
            OnPropertyChanged("TitleInfoEntries");

            MainWindowViewModel.Instance.CacheUpdateEnabled = true;
            MainWindowViewModel.Instance.RaisePropertyChangedEvent("CacheUpdateEnabled");
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}