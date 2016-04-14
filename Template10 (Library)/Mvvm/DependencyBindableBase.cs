using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Template10.Common;
using Template10.Utils;

namespace Template10.Mvvm
{
    public abstract class DependencyBindableBase : DependencyObject, IBindable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (object.Equals(storage, value))
                return false;

            storage = value;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        public virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;

            var handler = PropertyChanged;
            if (!object.Equals(handler, null))
            {
                var args = new PropertyChangedEventArgs(propertyName);
                var dispatcher = WindowWrapper.Current().Dispatcher;
                if (dispatcher.HasThreadAccess())
                {
                    try
                    {
                        handler.Invoke(this, args);
                    }
                    catch
                    {
                        dispatcher.Dispatch(() => handler.Invoke(this, args));
                    }
                }
                else
                {
                    dispatcher.Dispatch(() => handler.Invoke(this, args));
                }
            }
        }

        public virtual bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
        {
            if (object.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            this.RaisePropertyChanged(propertyExpression);
            return true;
        }

        public virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;

            var handler = this.PropertyChanged;
            if (!object.Equals(handler, null))
            {
                var propertyName = ExpressionUtils.GetPropertyName(propertyExpression);
                if (!object.Equals(propertyName, null))
                {
                    var args = new PropertyChangedEventArgs(propertyName);
                    var dispatcher = WindowWrapper.Current().Dispatcher;
                    if (dispatcher.HasThreadAccess())
                    {
                        try
                        {
                            handler.Invoke(this, args);
                        }
                        catch
                        {
                            dispatcher.Dispatch(() => handler.Invoke(this, args));
                        }
                    }
                    else
                    {
                        dispatcher.Dispatch(() => handler.Invoke(this, args));
                    }
                }
            }
        }
    }
}