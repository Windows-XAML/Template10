using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Template10.Mvvm
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-MVVM
    public abstract class BindableBase : IBindable
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        private PropertyChangedEventHandler _handler;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                // somebody is adding!
                _handler += value;
            }
            remove
            {
                // somebody is removing!
                _handler -= value;
            }
        }
        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;
            try
            {
                _handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch
            {
                // nothing
            }
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (object.Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
