using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.ViewService;
using Template10.Services.WindowWrapper;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.NavigationService
{
	public interface INavigationService
    {
        event EventHandler<NavigatedEventArgs> Navigated;
        void RaiseNavigated(NavigatedEventArgs e);

        event EventHandler<NavigatingEventArgs> Navigating;
        void RaiseNavigating(NavigatingEventArgs e);

        event EventHandler<HandledEventArgs> BackRequested;
        void RaiseBackRequested(HandledEventArgs args);

        event EventHandler<HandledEventArgs> ForwardRequested;
        void RaiseForwardRequested(HandledEventArgs args);

        void GoBack(NavigationTransitionInfo infoOverride = null);
        Task<bool> GoBackAsync(NavigationTransitionInfo infoOverride = null);

        SerializationService.ISerializationService SerializationService { get; set; }

        IWindowWrapper Window { get; }


        void GoForward();
        Task<bool> GoForwardAsync();

        object Content { get; }

        void Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
        void Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;

        Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
        Task<bool> NavigateAsync<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;

        bool CanGoBack { get; }
        bool CanGoForward { get; }

        string NavigationState { get; set; }

        void Refresh();
        void Refresh(object param);

        Task<bool> RefreshAsync();
        Task<bool> RefreshAsync(object param);

        ISuspensionStateLogic Suspension { get; }

        Task<IViewLifetimeControl> OpenAsync(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf);

        object CurrentPageParam { get; }
        Type CurrentPageType { get; }

        IDispatcherWrapper Dispatcher { get; }

        Task SaveAsync();

        Task<bool> LoadAsync();

        Common.BackButton BackButtonHandling { get; set; }

        event TypedEventHandler<Type> AfterRestoreSavedNavigation;

        void ClearHistory();
        void ClearCache(bool removeCachedPagesInBackStack = false);

        Task SuspendingAsync();
        void Resuming();

        [Obsolete("Use FrameFacade. This may be made private in future versions.", false)]
        Frame Frame { get; }
        IFrameFacade FrameFacade { get; }

        /// <summary>
        /// Specifies if this instance of INavigationService associated with <see cref="CoreApplication.MainView"/> or any other secondary view.
        /// </summary>
        /// <returns><value>true</value> if associated with MainView, <value>false</value> otherwise</returns>
        bool IsInMainView { get; }
    }
}