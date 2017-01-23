// Project: MapleSeedU
// File: TitleInfoViewModel.cs
// Updated By: Jared
// 

#region usings

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using MapleSeedU.Models;

#endregion

namespace MapleSeedU.ViewModels
{
    public class TitleInfoViewModel : INotifyPropertyChanged
    {
        public TitleInfoViewModel()
        {
            var path = MainWindowViewModel.Instance.LibraryPath.GetPath();
            var files = Directory.EnumerateFiles(path, "*.rpx", SearchOption.AllDirectories);

            IList<TitleInfoEntry> list = files.Select(file => new TitleInfoEntry(file)).ToList();

            TitleInfoEntries = new CollectionView(list);
        }

        public CollectionView TitleInfoEntries { get; }
        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}