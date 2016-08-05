using System;
using System.ComponentModel;

namespace JSONPlaceHolder
{
    /// <summary>
    /// base class for ViewModelBase
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Raise property change
        /// </summary>
        /// <param name="property"></param>
        protected virtual void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        protected abstract void Initialize();
    }
}
