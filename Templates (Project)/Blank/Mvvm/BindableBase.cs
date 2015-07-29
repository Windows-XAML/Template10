﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Common;

namespace Template10.Mvvm
{
    public abstract class BindableBase : INotifyPropertyChanged, IBindable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public async void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;
            await WindowWrapper.Current().Dispatcher.DispatchAsync(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
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
