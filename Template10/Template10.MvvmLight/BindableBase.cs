using System.Runtime.CompilerServices;
using System;
using System.Linq.Expressions;

namespace Template10.Mvvm
{
    public abstract class BindableBase : GalaSoft.MvvmLight.ObservableObject
    {
        public sealed override void RaisePropertyChanged([CallerMemberName] string propertyName = null) => base.RaisePropertyChanged(propertyName);

        public sealed override void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression) => base.RaisePropertyChanged(propertyExpression);

        public new bool Set<T>(Expression<Func<T>> propertyExpression, ref T storage, T value) => base.Set(propertyExpression, ref storage, value);

        protected new bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) => base.Set(ref storage, value, propertyName);

        public void Set(Action action, [CallerMemberName] string propertyName = null)
        {
            try
            {
                action.Invoke();
            }
            finally
            {
                RaisePropertyChanged(propertyName);
            }
        }
    }
}