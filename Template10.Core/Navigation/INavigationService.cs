using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.View;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Navigation
{
    public interface INavigationService
    {
        Task<ILifetimeService> OpenAsync(Type page, object parameter = null, string title = null, Windows.UI.ViewManagement.ViewSizePreference size = Windows.UI.ViewManagement.ViewSizePreference.UseHalf);

        Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
        Task<bool> NavigateAsync<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;

        bool CanGoBack { get; }
        void GoBack(NavigationTransitionInfo infoOverride = null);

        bool CanGoForward { get; }
        void GoForward();

        INavigationState NavigationState { get; }
        Suspension.ISuspensionState SuspensionState { get; }

        INavigationItem Current { get; }
        INavigationItems BackStack { get; }
        INavigationItems ForwardStack { get; }

        Windows.UI.Xaml.Controls.Page Page { get; }
        object ViewModel { get; }
    }
}

