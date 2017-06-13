using System.Threading.Tasks;
using Template10.Portable.State;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public interface INavigatedAwareAsync
    {
        Task OnNavigatedToAsync(object parameter, NavigationMode mode, IPersistedStateContainer state);
        Task OnNavigatedFromAsync(IPersistedStateContainer suspensionState, bool suspending);
    }
}