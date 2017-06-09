using System;
using System.Collections.Specialized;
using System.ComponentModel;
using Template10.Common;

namespace Template10.Controls
{
    public interface IObservableItemCollection<T> : INotifyCollectionChanged, IDisposable where T : INotifyPropertyChanged
    {
        event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;
    }
}