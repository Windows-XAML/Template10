using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Template10.Samples.MvvmLightSample.Mvvm
{
    public abstract class ViewModelBase :
         GalaSoft.MvvmLight.ViewModelBase, Template10.Services.NavigationService.INavigable
    {
        public virtual Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnNavigatingFromAsync(Template10.Services.NavigationService.NavigatingEventArgs args)
        {
            return Task.CompletedTask;
        }

        [JsonIgnore]
        public Template10.Services.NavigationService.INavigationService NavigationService { get; set; }

        [JsonIgnore]
        public Template10.Common.IDispatcherWrapper Dispatcher { get; set; }

        [JsonIgnore]
        public Template10.Common.IStateItems SessionState { get; set; }
    }
}
