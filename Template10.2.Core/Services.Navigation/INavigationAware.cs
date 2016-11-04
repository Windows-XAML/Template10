using System.Threading.Tasks;

namespace Template10.Services.Navigation
{
    public interface INavigationAware : INavigatedAware, INavigatingAware
    {
        // nothing
    }

    public interface INavigatingAware
    {
        Task OnNavigatingToAsync(INavigationParameter parameters, NavigationModes mode);

        Task OnNavigatingFromAsync();
    }

    public interface INavigatedAware
    {
        Task OnNavigatedToAsync(INavigationParameter parameters, NavigationModes mode);

        Task OnNavigatedFromAsync();
    }

    public interface ISuspensionAware
    {
        Task OnResumingAsync(ISuspensionState state);

        Task OnSuspendingAsync(ISuspensionState state);
    }

    public interface IConfirmNavigation
    {
        Task<bool> CanNavigateAsync();
    }
}
