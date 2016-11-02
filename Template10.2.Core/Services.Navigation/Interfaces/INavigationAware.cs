using System.Threading.Tasks;

namespace Template10.Services.Navigation
{
    public interface INavigationAware
    {
        Task OnNavigatedToAsync(string parameter, NavigationModes mode);
        Task OnNavigatedFromAsync();
    }
}
