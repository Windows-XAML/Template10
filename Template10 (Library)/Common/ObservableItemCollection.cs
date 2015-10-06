using System;
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
    public class ObservableItemCollection<T> : ObservableCollection<T>
    {
        public ObservableItemCollection()
        {
            base.CollectionChanged += (s, e) =>
            {
                if (_enableCollectionChanged)
                    CollectionChanged?.Invoke(this, e);
            };
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (INotifyPropertyChanged item in e.NewItems)
                    {
                        item.PropertyChanged += itemPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    foreach (INotifyPropertyChanged item in e.OldItems)
                    {
                        item.PropertyChanged -= itemPropertyChanged;
                    }
                    if (e.NewItems != null)
                    {
                        foreach (INotifyPropertyChanged item in e.NewItems)
                        {
                            item.PropertyChanged += itemPropertyChanged;
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
            }
            base.OnCollectionChanged(e);
        }

        private void itemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ItemPropertyChanged?.Invoke(sender, e);
        }
        public event EventHandler<PropertyChangedEventArgs> ItemPropertyChanged;

        public void AddRange(IEnumerable<T> items)
        {
            _enableCollectionChanged = false;
            foreach (var item in items)
            {
                this.Add(item);
            }
            _enableCollectionChanged = true;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            _enableCollectionChanged = false;
            foreach (var item in items)
            {
                this.Remove(item);
            }
            _enableCollectionChanged = true;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items));
        }

        private bool _enableCollectionChanged = true;
        public override event NotifyCollectionChangedEventHandler CollectionChanged;
    }

}
