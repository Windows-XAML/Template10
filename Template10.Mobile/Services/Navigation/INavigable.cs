using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template10.Mobile.Services.NavigationService
{
    public interface INavigable : INavigatedAwareAsync, INavigatingAwareAsync
    {
    }

    public interface INavigatedAwareAsync
    {
        Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode, IDictionary<string, object> suspensionState);
        Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending);
    }

    public interface INavigatingAwareAsync
    {
        Task OnNavigatingFromAsync(NavigatingEventArgs args);
    }
}