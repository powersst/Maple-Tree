// Project: MapleSeedU
// File: ObservableObject.cs
// Updated By: Jared
// 

#region usings

using System.ComponentModel;

#endregion

namespace MapleSeedU.ViewModels
{
    public class ObservableObject : INotifyPropertyChanged
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
}