using System.Threading.Tasks;
using Template10.Portable.PersistedDictionary;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public interface INavigatedAwareAsync
    {
        Task OnNavigatedToAsync(object parameter, NavigationMode mode, IPersistedDictionary state);
        Task OnNavigatedFromAsync(IPersistedDictionary suspensionState, bool suspending);
    }
}