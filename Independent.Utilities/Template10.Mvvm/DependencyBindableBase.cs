using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Template10.Utils;
using Windows.UI.Xaml;

namespace Template10.Mvvm
{
    public abstract class DependencyBindableBase : DependencyObject, IBindable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        public virtual bool Set<T>(Expression<Func<T>> propertyExpression, ref T storage, T value)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            RaisePropertyChanged(propertyExpression);
            return true;
        }

        public virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;
            var handler = PropertyChanged;
            if (Equals(handler, null)) return;
            var args = new PropertyChangedEventArgs(propertyName);
            handler.Invoke(this, args);
        }

        public virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;
            var handler = PropertyChanged;
            if (!Equals(handler, null))
            {
                var propertyName = ExpressionUtils.GetPropertyName(propertyExpression);
                if (Equals(propertyName, null)) return;
                var args = new PropertyChangedEventArgs(propertyName);
                handler.Invoke(this, args);
            }
        }
    }
}