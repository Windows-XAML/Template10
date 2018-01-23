using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Navigation
{
    public interface INavigationServiceUwp : INavigationService
    {
        Task RefreshAsync();

        bool CanGoBack();
        event EventHandler CanGoBackChanged;

        bool CanGoForward();
        event EventHandler CanGoForwardChanged;

        Task<INavigationResult> GoBackAsync(INavigationParameters parameters, NavigationTransitionInfo infoOverride);
        Task<INavigationResult> NavigateAsync(string path, INavigationParameters parameter, NavigationTransitionInfo infoOverride);
        Task<INavigationResult> NavigateAsync(Uri path, INavigationParameters parameter, NavigationTransitionInfo infoOverride);

        Dictionary<string, object> SessionState { get; }

    }
}
