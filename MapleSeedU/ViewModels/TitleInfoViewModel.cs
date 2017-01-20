// Project: MapleSeedU
// File: TitleInfoViewModel.cs
// Updated By: Jared
// 

#region usings

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using MapleSeedU.Models;

#endregion

namespace MapleSeedU.ViewModels
{
    public class TitleInfoViewModel : INotifyPropertyChanged
    {
        private string _titleInfoEntry;

        public TitleInfoViewModel()
        {
            IList<TitleInfoEntry> list = new List<TitleInfoEntry>();

            var path = Presenter.LibraryPath.GetPath();
            var files = Directory.GetFiles(path, "*.rpx", SearchOption.AllDirectories);
            foreach (var file in files) {
                
            }

            list.Add(new TitleInfoEntry("test"));
            list.Add(new TitleInfoEntry("test2"));

            TitleInfoEntries = new CollectionView(list);
        }

        public CollectionView TitleInfoEntries { get; }

        public string TitleInfoEntry {
            get { return _titleInfoEntry; }
            set {
                if (_titleInfoEntry == value) return;
                _titleInfoEntry = value;
                OnPropertyChanged("TitleInfoEntry");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}