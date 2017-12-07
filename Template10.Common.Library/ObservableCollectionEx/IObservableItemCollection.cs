using System;
using System.Collections.Specialized;
using System.ComponentModel;
using Template10.Common;

namespace Template10.Controls
{
    public interface IObservableCollectionEx<T> : INotifyCollectionChanged, IDisposable 
    {
        event EventHandler<ObservableCollectionExItemPropertyChangedEventArgs> ItemPropertyChanged;
    }
}