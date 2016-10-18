using System.Threading.Tasks;

namespace Template10.Navigation
{
    public interface INavigationAware
    {
        Task OnNavigatedToAsync(string parameter, NavigationModes mode, INavigationState state);
        Task OnNavigatedFromAsync();
    }
}
