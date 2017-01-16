// Project: MapleSeedU
// File: Presenter.cs
// Updated By: Jared
// 

#region usings

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MapleSeedU.Models;

#endregion

namespace MapleSeedU.ViewModels
{
    public class Presenter : ObservableObject
    {
        private readonly ObservableCollection<string> _history = new ObservableCollection<string>();
        private readonly TextConverter _textConverter = new TextConverter(s => s.ToUpper());
        private string _someText;

        public string SomeText {
            get { return _someText; }
            set {
                _someText = value;
                RaisePropertyChangedEvent("SomeText");
            }
        }

        public IEnumerable<string> History => _history;

        public ICommand ConvertTextCommand => new DelegateCommand(ConvertText);

        private void ConvertText()
        {
            if (string.IsNullOrWhiteSpace(SomeText)) return;
            AddToHistory(_textConverter.ConvertText(SomeText));
            SomeText = string.Empty;
        }

        private void AddToHistory(string item)
        {
            if (!_history.Contains(item))
                _history.Add(item);
        }
    }
}