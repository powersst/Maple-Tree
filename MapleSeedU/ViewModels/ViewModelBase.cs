// Project: MapleSeedU
// File: ViewModelBase.cs
// Updated By: Jared
// 

#region usings

using System;
using System.ComponentModel;
using System.Windows.Input;

#endregion

namespace MapleSeedU.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        protected ViewModelBase()
        {
            _canExecute = true;
        }

        private readonly bool _canExecute;
        private ICommand _clickCommand;
        public ICommand ClickCommand => _clickCommand ?? (_clickCommand = new CommandHandler(MyAction, _canExecute));
        protected abstract void MyAction();

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

        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }

        #endregion
    }
}