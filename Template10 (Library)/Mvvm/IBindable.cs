using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Template10.Mvvm
{
    // this exists for the future implementation of the INPC property attribute
    public interface IBindable : INotifyPropertyChanged
    {
        void RaisePropertyChanged([CallerMemberName]string propertyName = null);
   

        void Set<T>(
            T oldValue,
            T newValue,
            out T storage,
            Func<T, T, bool> equalCompare=null,
            [CallerMemberName]string propertyName = null);

        void Set<T>(
            ref T storage,
            T value,
            Func<T, T, bool> equalCompare = null,
            [CallerMemberName]string propertyName = null);
    }
}
