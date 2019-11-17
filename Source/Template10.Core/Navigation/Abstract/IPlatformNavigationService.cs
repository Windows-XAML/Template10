using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Navigation
{
    public interface INavigationService
    {
        Task<INavigationResult> GoBackAsync();
        Task<INavigationResult> GoBackAsync(INavigationParameters parameters);
        Task<INavigationResult> NavigateAsync(Uri uri);
        Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameters);
        Task<INavigationResult> NavigateAsync(string name);
        Task<INavigationResult> NavigateAsync(string name, INavigationParameters parameters);

        Task RefreshAsync();

        bool CanGoBack();
        event EventHandler CanGoBackChanged;
        Task<INavigationResult> GoBackAsync(INavigationParameters parameters, NavigationTransitionInfo infoOverride);

        bool CanGoForward();
        event EventHandler CanGoForwardChanged;
        Task<INavigationResult> GoForwardAsync();
        Task<INavigationResult> GoForwardAsync(INavigationParameters parameter);

        Task<INavigationResult> NavigateAsync(string path, INavigationParameters parameter, NavigationTransitionInfo infoOverride);
        Task<INavigationResult> NavigateAsync(Uri path, INavigationParameters parameter, NavigationTransitionInfo infoOverride);
    }
}
