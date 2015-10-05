using System;
using System.Collections.Generic;
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

        public bool Set<T>(T previous, T value, out T storage, Func<T, T, bool> equalCompare, [CallerMemberName] string propertyName = null)
        {
            bool raised;
            storage = this.CompareAndRaiseEventIfChanged(previous, value, equalCompare, propertyName, out raised);
            return raised;
        }

        public bool Set<T>(ref T storage, T value, Func<T, T, bool> equalCompare, [CallerMemberName] string propertyName = null)
        {
            return Set(storage, value, out storage, equalCompare, propertyName);
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            return Set(storage, value, out storage, null, propertyName);
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

        public bool Set<T>(T previous, T value, out T storage, Func<T, T, bool> comparer = null, [CallerMemberName] string propertyName = null)
        {
            bool raised;
            storage = this.CompareAndRaiseEventIfChanged(previous, value, comparer, propertyName, out raised);
            return raised;
        }

        public bool Set<T>(ref T storage, T value, Func<T, T, bool> comparer, [CallerMemberName] string propertyName = null)
        {
            return Set(storage, value, out storage, comparer, propertyName);
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            return Set(storage, value, out storage, null, propertyName);
        }
    }

    internal static class BindableHelper
    {
        public static T CompareAndRaiseEventIfChanged<T>(this IBindable model, T oldValue, T newValue, Func<T, T, bool> equalCompare, string propertyName, out bool raised)
        {
            T setToField;
            bool isEq;
            if (equalCompare != null)
            {
                isEq = equalCompare(oldValue, newValue);
            }
            else
            {
                var cpr = EqualityComparer<T>.Default;
                isEq = cpr.Equals(oldValue, newValue);
            }

            if (raised = !isEq)
            {
                setToField = newValue;
                model.RaisePropertyChanged(propertyName);
            }
            else
            {
                setToField = oldValue;
            }

            return setToField;
        }


    }


}
