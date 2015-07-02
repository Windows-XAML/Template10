using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public interface INavigatable
    {
        void OnNavigatedTo(string parameter, NavigationMode mode, IDictionary<string, object> state);
        Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending);
        void OnNavigatingFrom(Services.NavigationService.NavigatingEventArgs args);
    }
}
