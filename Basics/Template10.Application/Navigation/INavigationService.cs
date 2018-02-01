using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Navigation
{
    public interface IPlatformNavigationService : INavigationService
    {
        Task RefreshAsync();

        bool CanGoBack();
        event EventHandler CanGoBackChanged;
        Task<INavigationResult> GoBackAsync(INavigationParameters parameters, NavigationTransitionInfo infoOverride);

        bool CanGoForward();
        event EventHandler CanGoForwardChanged;
        Task<INavigationResult> GoForwardAsync();

        Task<INavigationResult> NavigateAsync(string path, INavigationParameters parameter, NavigationTransitionInfo infoOverride);
        Task<INavigationResult> NavigateAsync(Uri path, INavigationParameters parameter, NavigationTransitionInfo infoOverride);
    }
}
