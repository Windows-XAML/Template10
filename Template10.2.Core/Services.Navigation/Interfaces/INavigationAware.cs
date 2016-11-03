using System.Threading.Tasks;

namespace Template10.Services.Navigation
{
    public interface INavigatingAware : INavigationAware
    {
        Task OnNavigatingToAsync(string parameter, NavigationModes mode);
        Task OnNavigatingFromAsync();
    }

    public interface INavigationAware
    {
        Task OnNavigatedToAsync(string parameter, NavigationModes mode);
        Task OnNavigatedFromAsync();
    }
}
