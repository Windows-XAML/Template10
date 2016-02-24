using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Controls
{
    public class ObservableItemCollection<T> : ObservableCollection<T>, IDisposable where T : INotifyPropertyChanged
    {
        private bool _enableCollectionChanged = true;
        public override event NotifyCollectionChangedEventHandler CollectionChanged;
        public event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;

        public ObservableItemCollection()
        {
            base.CollectionChanged += (s, e) =>
            {
                if (_enableCollectionChanged)
                    CollectionChanged?.Invoke(this, e);
            };
        }

        public ObservableItemCollection(IEnumerable<T> collection) : base(collection)
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
            foreach (INotifyPropertyChanged item in items)
            {
                if (item != null)
                {
                    item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
        }

        private void UnRegisterPropertyChanged(IList items)
        {
            CheckDisposed();
            foreach (INotifyPropertyChanged item in items)
            {
                if (item != null)
                {
                    item.PropertyChanged -= new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CheckDisposed();
            ItemPropertyChanged?.Invoke(this, new ItemPropertyChangedEventArgs(sender, IndexOf((T)sender), e));
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

        bool disposed = false;

        public void Dispose()
        {
            if (!disposed)
            {
                ClearItems();
                disposed = true;
            }
        }

        public void CheckDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }

}
