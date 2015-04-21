using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Template10.Mvvm
{
    // viewmodelbase enables To/From, called by NavigationService
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        public virtual void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state)
        {
        }

        public virtual void OnNavigatedFrom(Dictionary<string, object> state, bool suspending)
        {
        }
    }
}