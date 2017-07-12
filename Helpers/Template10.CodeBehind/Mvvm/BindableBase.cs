using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Template10.Utils;

namespace Template10.Mvvm
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/MVVM
    public abstract class BindableBase : IBindable
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;
            var propertyName = ExpressionUtils.GetPropertyName(propertyExpression);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
