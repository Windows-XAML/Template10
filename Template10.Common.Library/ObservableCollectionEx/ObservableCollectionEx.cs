using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Template10.Common;

namespace Template10.Controls
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>, IObservableCollectionEx<T> 
    {
        private bool _enableCollectionChanged = true;
        public override event NotifyCollectionChangedEventHandler CollectionChanged;
        public event EventHandler<ObservableCollectionExItemPropertyChangedEventArgs> ItemPropertyChanged;

        public ObservableCollectionEx()
        {
            base.CollectionChanged += (s, e) =>
            {
                if (_enableCollectionChanged)
                {
                    CollectionChanged?.Invoke(this, e);
                }
            };
        }

        public ObservableCollectionEx(IEnumerable<T> collection) : base(collection)
        {
            base.CollectionChanged += (s, e) =>
            {
                if (_enableCollectionChanged)
                {
                    CollectionChanged?.Invoke(this, e);
                }
            };
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CheckDisposed();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RegisterPropertyChanged(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    UnRegisterPropertyChanged(e.OldItems);
                    if (e.NewItems != null)
                    {
                        RegisterPropertyChanged(e.NewItems);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
            }
            base.OnCollectionChanged(e);
        }

        private void RegisterPropertyChanged(IList items)
        {
            CheckDisposed();
            foreach (var item in items.OfType<INotifyPropertyChanged>())
            {
                if (item != null)
                {
                    item.PropertyChanged += new PropertyChangedEventHandler(Item_PropertyChanged);
                }
            }
        }

        private void UnRegisterPropertyChanged(IList items)
        {
            CheckDisposed();
            foreach (var item in items.OfType<INotifyPropertyChanged>())
            {
                if (item != null)
                {
                    item.PropertyChanged -= new PropertyChangedEventHandler(Item_PropertyChanged);
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CheckDisposed();
            ItemPropertyChanged?.Invoke(this, new ObservableCollectionExItemPropertyChangedEventArgs(sender, IndexOf((T)sender), e));
        }


        public void AddRange(IEnumerable<T> items)
        {
            CheckDisposed();
            _enableCollectionChanged = false;
            foreach (var item in items)
            {
                Add(item);
            }
            _enableCollectionChanged = true;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            CheckDisposed();
            _enableCollectionChanged = false;
            foreach (var item in items)
            {
                Remove(item);
            }
            _enableCollectionChanged = true;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items));
        }

        protected override void ClearItems()
        {
            UnRegisterPropertyChanged(this);
            base.ClearItems();
        }

        bool _disposed = false;

        public void Dispose()
        {
            if (!_disposed)
            {
                ClearItems();
                _disposed = true;
            }
        }

        public void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }

}
