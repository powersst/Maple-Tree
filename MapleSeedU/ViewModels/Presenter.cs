// Project: MapleSeedU
// File: Presenter.cs
// Updated By: Jared
// 

#region usings

#endregion

#region usings

using System.Collections.Generic;
using MapleSeedU.Models;

#endregion

namespace MapleSeedU.ViewModels
{
    public class Presenter : ObservableObject
    {
        public Presenter()
        {

        }

        private string _status = "Github.com/Tsume/Maple-Tree";
        private List<TitleInfo> _titleInfoLife = new List<TitleInfo> {new TitleInfo {Name = "No Titles Present"}};
        private string _titleName = "No Game Selected";

        public List<TitleInfo> TitleInfoList {
            get { return _titleInfoLife; }
            set {
                _titleInfoLife = value;
                RaisePropertyChangedEvent("TitleInfoList");
            }
        }

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