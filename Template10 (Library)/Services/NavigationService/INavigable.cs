using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Common;

namespace Template10.Services.NavigationService
{
    public interface INavigable : ITemplate10ViewModel, INavigatedAwareAsync, INavigatingAwareAsync
    {
    }

    public interface ITemplate10ViewModel
    {
        INavigationService NavigationService { get; set; }
        IDispatcherWrapper Dispatcher { get; set; }
        IStateItems SessionState { get; set; }
    }

    public interface INavigatedAwareAsync
    {
        Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state);
        Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending);
    }

    public interface INavigatingAwareAsync
    {
        Task OnNavigatingFromAsync(NavigatingEventArgs args);
    }
}