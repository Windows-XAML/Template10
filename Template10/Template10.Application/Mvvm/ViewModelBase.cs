using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Template10.Common;
using Template10.Services.NavigationService;
using Template10.Portable.Navigation;
using Template10.Services.WindowWrapper;
using Template10.Utils;

namespace Template10.Mvvm
{
    public abstract class Template10ViewModel
        : BindableBase,
        INavigatedToAwareAsync,
        INavigatedFromAwareAsync,
        IConfirmNavigationAsync,
        ITemplate10ViewModel
    {
        public abstract Task OnNavigatedToAsync(INavigatedToParameters parameter);

        public async virtual Task OnNavigatedFromAsync(INavigatedFromParameters parameters)
        {
            await Task.CompletedTask;
        }

        public async virtual Task<bool> CanNavigateAsync(IConfirmNavigationParameters parameters)
        {
            await Task.CompletedTask;
            return true;
        }

        [JsonIgnore]
        public virtual IWindowWrapper Window => NavigationService.GetWindowWrapper();

        [JsonIgnore]
        public virtual IDispatcherWrapper Dispatcher => Window.Dispatcher;

        [JsonIgnore]
        public virtual INavigationService NavigationService { get; set; }

        [JsonIgnore]
        public virtual IDictionary<string, object> SessionState => SessionStateHelper.Current;
    }
}