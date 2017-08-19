using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Template10.Mvvm
{
    public abstract class BindableBase : GalaSoft.MvvmLight.ObservableObject
    {
        public override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);
        }

        public override void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            base.RaisePropertyChanged(propertyExpression);
        }

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