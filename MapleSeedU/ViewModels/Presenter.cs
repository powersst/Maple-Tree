// Project: MapleSeedU
// File: Presenter.cs
// Updated By: Jared
// 

#region usings

using MapleSeedU.Models;

#endregion

namespace MapleSeedU.ViewModels
{
    public class Presenter : ObservableObject
    {
        private string _status = "Github.com/Tsume/Maple-Tree";
        private string _titleName = "No Game Selected";

        public Presenter()
        {
            Status = LibraryPath.GetPath();
        }

        private LibraryPath LibraryPath { get; set; } = new LibraryPath();

        public TitleInfoViewModel TitleInfoViewModel { get; set; } = new TitleInfoViewModel();

        public string Status {
            get { return _status; }
            set {
                _status = value;
                RaisePropertyChangedEvent("Status");
            }
        }

        public string TitleName {
            get { return _titleName; }
            set {
                _titleName = value;
                RaisePropertyChangedEvent("TitleName");
            }
        }
    }
}