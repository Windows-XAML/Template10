using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Template10.Mvvm
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-MVVM
    public abstract class ViewModelBase : BindableBase, INavigable
    {
        public virtual void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state) { /* nothing by default */ }
        public virtual async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            await Task.CompletedTask;
        }
        public virtual void OnNavigatingFrom(Services.NavigationService.NavigatingEventArgs args) { /* nothing by default */ }

        [JsonIgnore]
        public INavigationService NavigationService { get; set; }
        [JsonIgnore]
        public IDispatcherWrapper Dispatcher { get; set; }
        [JsonIgnore]
        public IStateItems SessionState { get; set; }
        
    }
}