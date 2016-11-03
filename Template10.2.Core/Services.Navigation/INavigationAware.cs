using System.Threading.Tasks;

namespace Template10.Services.Navigation
{
    public interface INavigationAware : INavigatedAware, INavigatingAware
    {
        // nothing
    }

    public interface INavigatingAware
    {
        Task OnNavigatingToAsync(INavigationParameters parameters, NavigationModes mode);

        Task OnNavigatingFromAsync();
    }

    public interface INavigatedAware
    {
        Task OnNavigatedToAsync(INavigationParameters parameters, NavigationModes mode);

        Task OnNavigatedFromAsync();
    }

    public interface ISuspensionAware
    {
        Task OnResumingAsync(INavigationParameters state);

        Task OnSuspendingAsync(INavigationParameters state);
    }

    public interface IConfirmNavigation
    {
        Task<bool> CanNavigateAsync();
    }
}
