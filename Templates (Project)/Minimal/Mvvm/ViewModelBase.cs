using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Template10.Mvvm
{
    public abstract class ViewModelBase : BindableBase, INavigable
    {
        public string Identifier { get; set; }
        public NavigationService NavigationService { get; set; }
        public virtual void OnNavigatedTo(string parameter, NavigationMode mode, IDictionary<string, object> state) { /* nothing by default */ }
        public virtual Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending) { return Task.FromResult<object>(null); }
        public virtual void OnNavigatingFrom(Services.NavigationService.NavigatingEventArgs args) { /* nothing by default */ }
    }
}