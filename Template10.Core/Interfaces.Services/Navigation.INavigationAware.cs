using System.Threading.Tasks;

namespace Template10.Interfaces.Services.Navigation
{
    public interface INavigationAware
    {
        Task OnNavigatedToAsync(string parameter, NavigationModes mode, INavigationState state);
        Task OnNavigatedFromAsync();
    }
}
