using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Template10.Mvvm
{
    public interface IBindable: INotifyPropertyChanged
    {
        void RaisePropertyChanged([CallerMemberName]string propertyName = null);
        void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null);
    }
}