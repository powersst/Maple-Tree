// Project: MapleSeedU
// File: ViewModelBase.cs
// Updated By: Jared
// 

#pragma warning disable CS0067 // Naming Styles

#region usings

using System;
using System.ComponentModel;
using System.Windows.Input;

#endregion

namespace MapleSeedU.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CommandHandler : ICommand
    {
        private readonly Action _action;
        private readonly bool _canExecute;

        public CommandHandler(Action action, bool canExecute = true)
        {
            _action = action;
            _canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}