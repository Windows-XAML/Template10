using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.NavigationService
{
    public interface INavigationService
    {
        void GoBack();
        bool CanGoBack { get; }

        void GoForward();
        bool CanGoForward { get; }

        void Refresh();

        bool Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
        bool Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;
        Task OpenAsync(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf);

        object CurrentPageParam { get; }
        Type CurrentPageType { get; }
        DispatcherWrapper Dispatcher { get; }

        Task SaveNavigationAsync();
        Task<bool> RestoreSavedNavigationAsync();
        event TypedEventHandler<Type> AfterRestoreSavedNavigation;

        void ClearHistory();
        void ClearCache(bool removeCachedPagesInBackStack = false);

        Task SuspendingAsync();
        void Resuming();

        Frame Frame { get; }
        FrameFacade FrameFacade { get; }
    }
}