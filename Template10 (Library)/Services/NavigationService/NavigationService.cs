using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public class NavigationService
    {
        private const string EmptyNavigation = "1,0";

        public FrameFacade FrameFacade { get; private set; }
        public Frame Frame { get { return FrameFacade.Frame; } }
        object LastNavigationParameter { get; set; }
        string LastNavigationType { get; set; }

        public DispatcherWrapper Dispatcher { get { return WindowWrapper.Current(this).Dispatcher; } }

        internal NavigationService(Frame frame)
        {
            FrameFacade = new FrameFacade(frame);
            FrameFacade.Navigating += async (s, e) =>
            {
                if (e.Suspending)
                    return;

                // allow the viewmodel to cancel navigation
                e.Cancel = !NavigatingFrom(false);
                if (!e.Cancel)
                {
                    await NavigateFromAsync(false);
                }
            };
            FrameFacade.Navigated += (s, e) =>
            {
                NavigateTo(e.NavigationMode, e.Parameter);
            };
        }

        // before navigate (cancellable)
        bool NavigatingFrom(bool suspending)
        {
            var page = FrameFacade.Content as Page;
            if (page != null)
            {
                var dataContext = page.DataContext as INavigable;
                if (dataContext != null)
                {
                    var args = new NavigatingEventArgs
                    {
                        PageType = FrameFacade.CurrentPageType,
                        Parameter = FrameFacade.CurrentPageParam,
                        Suspending = suspending,
                    };
                    dataContext.OnNavigatingFrom(args);
                    return !args.Cancel;
                }
            }
            return true;
        }

        // after navigate
        async Task NavigateFromAsync(bool suspending)
        {
            var page = FrameFacade.Content as Page;
            if (page != null)
            {
                // call viewmodel
                var dataContext = page.DataContext as INavigable;
                if (dataContext != null)
                {
                    var pageState = FrameFacade.PageStateContainer(page.GetType());
                    await dataContext.OnNavigatedFromAsync(pageState, suspending);
                }
            }
        }

        void NavigateTo(NavigationMode mode, object parameter)
        {
            LastNavigationParameter = parameter;
            LastNavigationType = FrameFacade.Content.GetType().FullName;

            if (mode == NavigationMode.New)
            {
                FrameFacade.ClearFrameState();
            }

            var page = FrameFacade.Content as Page;
            if (page != null)
            {
                if (page.DataContext == null)
                {
                    // to support dependency injection, but keeping it optional.
                    var viewmodel = BootStrapper.Current.ResolveForPage(page.GetType(), this);
                    if (viewmodel != null)
                        page.DataContext = viewmodel;
                }

                // call viewmodel
                var dataContext = page.DataContext as INavigable;
                if (dataContext != null)
                {
                    // prepare for state load
                    dataContext.NavigationService = this;
                    var pageState = FrameFacade.PageStateContainer(page.GetType());
                    dataContext.OnNavigatedTo(parameter, mode, pageState);
                }
            }
        }

        public async Task OpenAsync(Type page, object parameter = null, string title = "New Window", ViewSizePreference size = ViewSizePreference.UseHalf)
        {
            var currentView = ApplicationView.GetForCurrentView();
            var newView = CoreApplication.CreateNewView();
            var dispatcher = new DispatcherWrapper(newView.Dispatcher);
            await dispatcher.DispatchAsync(async () =>
            {
                var newWindow = Window.Current;
                var newAppView = ApplicationView.GetForCurrentView();
                newAppView.Title = title;

                var frame = BootStrapper.Current.NavigationServiceFactory(BootStrapper.BackButton.Ignore, BootStrapper.ExistingContent.Exclude);
                frame.Navigate(page, parameter);
                newWindow.Content = frame.Frame;
                newWindow.Activate();

                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                    newAppView.Id,
                    ViewSizePreference.UseMinimum,
                    currentView.Id,
                    ViewSizePreference.UseMinimum);
            });
        }

        public bool Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (page.FullName.Equals(LastNavigationType)
                && parameter == LastNavigationParameter)
                return false;
            return FrameFacade.Navigate(page, parameter, infoOverride);
        }

        public void SaveNavigation()
        {
            if (CurrentPageType == null)
                return;

            var state = FrameFacade.PageStateContainer(GetType());
            if (state == null)
            {
                throw new InvalidOperationException("State container is unexpectedly null");
            }

            state["CurrentPageType"] = CurrentPageType.ToString();
            try { state["CurrentPageParam"] = CurrentPageParam; }
            catch
            {
                throw new Exception("Failed to serialize page parameter, override/implement ToString()");
            }
            state["NavigateState"] = FrameFacade?.GetNavigationState();
        }

        public event EventHandler AfterRestoreSavedNavigation;
        public bool RestoreSavedNavigation()
        {
            try
            {
                var state = FrameFacade.PageStateContainer(GetType());
                if (state == null || !state.Any() || !state.ContainsKey("CurrentPageType"))
                {
                    return false;
                }

                FrameFacade.CurrentPageType = Type.GetType(state["CurrentPageType"].ToString());
                FrameFacade.CurrentPageParam = state["CurrentPageParam"];
                FrameFacade.SetNavigationState(state["NavigateState"].ToString());
                NavigateTo(NavigationMode.Refresh, FrameFacade.CurrentPageParam);
                while (Frame.Content == null) { /* wait */ }
                AfterRestoreSavedNavigation?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch { return false; }
        }

        public void Refresh() { FrameFacade.Refresh(); }

        public void GoBack() { if (FrameFacade.CanGoBack) FrameFacade.GoBack(); }

        public bool CanGoBack { get { return FrameFacade.CanGoBack; } }

        public void GoForward() { FrameFacade.GoForward(); }

        public bool CanGoForward { get { return FrameFacade.CanGoForward; } }

        public void ClearHistory() { FrameFacade.Frame.BackStack.Clear(); }

        public void Resuming() { /* nothing */ }

        public async Task SuspendingAsync()
        {
            SaveNavigation();
            await NavigateFromAsync(true);
        }

        public void Show(SettingsFlyout flyout, string parameter = null)
        {
            if (flyout == null)
                throw new ArgumentNullException(nameof(flyout));
            var dataContext = flyout.DataContext as INavigable;
            if (dataContext != null)
            {
                dataContext.OnNavigatedTo(parameter, NavigationMode.New, null);
            }
            flyout.Show();
        }

        public Type CurrentPageType { get { return FrameFacade.CurrentPageType; } }
        public object CurrentPageParam { get { return FrameFacade.CurrentPageParam; } }
    }
}

