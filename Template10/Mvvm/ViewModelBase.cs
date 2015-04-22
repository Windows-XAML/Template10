using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Template10.Mvvm
{
    public abstract class ViewModelBase : BindableBase, Services.NavigationService.INavigatable
    {
        public virtual void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state) { /* nothing by default */ }

        public virtual void OnNavigatedFrom(Dictionary<string, object> state, bool suspending) { /* nothing by default */ }
    }
}