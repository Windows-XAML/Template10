using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Template10.Mvvm
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/MVVM
    public abstract class ViewModelBase : BindableBase, INavigable
    {
        public virtual Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state) => Task.CompletedTask;

        public virtual Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending) => Task.CompletedTask;

        public virtual Task OnNavigatingFromAsync(NavigatingEventArgs args) => Task.CompletedTask;

        [JsonIgnore]
        public virtual INavigationService NavigationService { get; set; }

        [JsonIgnore]
        public virtual IDispatcherWrapper Dispatcher { get; set; }

        [JsonIgnore]
        public virtual IStateItems SessionState { get; set; }
    }
}