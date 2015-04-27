using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public interface INavigatable
    {
        void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state);
        void OnNavigatedFrom(Dictionary<string, object> state, bool suspending);
        void OnNavigatingFrom(NavigatingCancelEventArgs args);
    }
}
