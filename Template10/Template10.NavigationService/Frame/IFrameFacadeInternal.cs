using System;
using Template10.Common;
using Template10.Portable.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.NavigationService
{
    public interface IFrameFacadeInternal: IFrameFacade
    {
        Frame Frame { get; set; }
        INavigationService NavigationService { get; set; }

        string GetNavigationState();
        void SetNavigationState(string state);

        void GoBack();
        void GoBack(NavigationTransitionInfo infoOverride);
        void GoForward();

        bool Navigate(Type page, object parameter, NavigationTransitionInfo info);

        void RaiseNavigated(NavigatedEventArgs e);
        void RaiseNavigating(NavigatingEventArgs e);
        void RaiseBackRequested(HandledEventArgs args);
        void RaiseForwardRequested(HandledEventArgs args);

        void ClearCache(bool removeCachedPagesInBackStack = false);
    }
}