using System;
using Template10.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.NavigationService
{
    public interface IFrameFacadeInternal: IFrameFacade
    {
        Frame Frame { get; set; }
        string GetNavigationState();
        void GoBack();
        void GoBack(NavigationTransitionInfo infoOverride);
        void GoForward();
        void SetNavigationState(string state);
        bool Navigate(Type page, object parameter, NavigationTransitionInfo info);
        void RaiseNavigated(NavigatedEventArgs e);
        void RaiseNavigating(NavigatingEventArgs e);
        void RaiseBackRequested(HandledEventArgs args);
        void RaiseForwardRequested(HandledEventArgs args);
        INavigationService NavigationService { get; set; }
        void ClearCache(bool removeCachedPagesInBackStack = false);
    }
}