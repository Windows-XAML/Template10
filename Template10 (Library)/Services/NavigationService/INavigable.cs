using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Common;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public interface INavigable
    {
        Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state);
        Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending);
        Task OnNavigatingFromAsync(NavigatingEventArgs args);
        INavigationService NavigationService { get; set; }
        IDispatcherWrapper Dispatcher { get; set; }
        IStateItems SessionState { get; set; }
    }
}
