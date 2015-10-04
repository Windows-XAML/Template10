using System;
using System.Collections;
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

        public void Set<T>(T oldValue, T newValue, out T setToField, Func<T, T, bool> equalCompare = null, [CallerMemberName] string propertyName = null)
        {
            IBindable model = this;
            setToField = this.CompareAndRiseEventIfChanged(oldValue, newValue, equalCompare, propertyName);
        }

        public void Set<T>(ref T oldValueField, T newValue, Func<T, T, bool> equalCompare = null, [CallerMemberName] string propertyName = null)
        {
            Set(oldValueField, newValue, out oldValueField, equalCompare, propertyName);
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
        public void Set<T>(T oldValue, T newValue, out T setToField, Func<T, T, bool> equalCompare = null, [CallerMemberName] string propertyName = null)
        {
            IBindable model = this;
            setToField = this.CompareAndRiseEventIfChanged(oldValue, newValue, equalCompare, propertyName);
        }

        public void Set<T>(ref T oldValueField, T newValue, Func<T, T, bool> equalCompare = null, [CallerMemberName] string propertyName = null)
        {
            Set(oldValueField, newValue, out oldValueField, equalCompare, propertyName);
        }

    }

    internal static class BindableHelper
    {
        public static T CompareAndRiseEventIfChanged<T>(this IBindable model, T oldValue, T newValue, Func<T, T, bool> equalCompare, string propertyName)
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

            if (!isEq)
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
