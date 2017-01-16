// Project: MapleSeedU
// File: DelegateCommand.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Windows.Input;

#endregion

namespace MapleSeedU.ViewModels
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        #region ICommand Members

        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged {
            add { }
            remove { }
        }
#pragma warning restore 67

        #endregion
    }
}