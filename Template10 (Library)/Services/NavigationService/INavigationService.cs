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
        bool CanGoBack { get; }
        bool CanGoForward { get; }
        object CurrentPageParam { get; }
        Type CurrentPageType { get; }
        DispatcherWrapper Dispatcher { get; }
        Frame Frame { get; }
        FrameFacade FrameFacade { get; }

        event TypedEventHandler<Type> AfterRestoreSavedNavigation;

		void ClearCache(bool removeCachedPagesInBackStack = false);
		void ClearHistory();
        void GoBack();
        void GoForward();
        bool Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
        bool Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;
        Task OpenAsync(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf);
        void Refresh();
        Task<bool> RestoreSavedNavigationAsync();
        void Resuming();
        Task SaveNavigationAsync();
        Task SuspendingAsync();
    }
}