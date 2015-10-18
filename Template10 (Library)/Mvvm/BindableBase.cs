using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace Template10.Mvvm
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-MVVM
    public abstract class BindableBase : IBindable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch
            {
                // nothing
            }
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (object.Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Sets property and RaisePropertyChanged
        /// Usage Set(Name, value, () => Name = value);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <param name="doSet"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Set<T>(T currentValue, T newValue, Action doSet, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(currentValue, newValue)) return false;
            doSet.Invoke();
            RaisePropertyChanged(propertyName);
            return true;
        }
    }

    public abstract class DependencyBindableBase : DependencyObject, IBindable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch
            {
                // nothing
            }
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (object.Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Sets property and RaisePropertyChanged
        /// Usage Set(data.Name, value, () => data.Name = value);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <param name="doSet"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Set<T>(T currentValue, T newValue, Action doSet, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(currentValue, newValue)) return false;
            doSet.Invoke();
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}