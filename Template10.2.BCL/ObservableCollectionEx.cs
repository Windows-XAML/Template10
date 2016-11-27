using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Template10.BCL
{
    public partial class ObservableItemCollectionEx<T>
        : ObservableCollection<T>, IDisposable where T : INotifyPropertyChanged
    {
        private bool _allowRaiseCollectionChanged = true;

        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        public event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;

        public ObservableItemCollectionEx()
        {
            base.CollectionChanged += (s, e) =>
            {
                if (_allowRaiseCollectionChanged)
                    CollectionChanged?.Invoke(this, e);
            };
        }

        public ObservableItemCollectionEx(IEnumerable<T> collection) : base(collection)
        {
            base.CollectionChanged += (s, e) =>
            {
                if (_allowRaiseCollectionChanged)
                {
                    CollectionChanged?.Invoke(this, e);
                }
            };
        }

        protected sealed override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
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

        protected override void ClearItems()
        {
            UnRegisterPropertyChanged(this);
            base.ClearItems();
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
            _allowRaiseCollectionChanged = false;
            foreach (var item in items)
            {
                Add(item);
            }
            _allowRaiseCollectionChanged = true;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            CheckDisposed();
            _allowRaiseCollectionChanged = false;
            foreach (var item in items)
            {
                Remove(item);
            }
            _allowRaiseCollectionChanged = true;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items));
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

        public class ItemPropertyChangedEventArgs
        {
            public ItemPropertyChangedEventArgs(object item, int changedIndex, PropertyChangedEventArgs e)
            {
                ChangedItem = item;
                ChangedItemIndex = changedIndex;
                PropertyChangedArgs = e;
            }

            public object ChangedItem { get; set; }

            public int ChangedItemIndex { get; set; }

            public PropertyChangedEventArgs PropertyChangedArgs { get; set; }
        }
    }
}
