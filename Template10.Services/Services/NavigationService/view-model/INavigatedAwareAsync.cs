using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public interface INavigatedAwareAsync
    {
        Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state);
        Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending);
    }
}