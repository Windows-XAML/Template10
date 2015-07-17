using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Template10.Mvvm
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public BindableBase()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // default to current dispatcher, developer must change for special-vase UI threads
                this.Dispatch = Template10.Common.WindowWrapper.Current().Dispatch;
            }
        }

        public Action<Action> Dispatch { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;
            Dispatch(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }

        public void Set<T>(ref T storage, T value, [CallerMemberName()]string propertyName = null)
        {
            if (object.Equals(storage, value))
                return;
            storage = value;
            RaisePropertyChanged(propertyName);
        }
    }
}
