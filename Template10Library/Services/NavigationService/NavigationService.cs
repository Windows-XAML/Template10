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

        // TODO: this will spawn a new window instead of navigating to an existing frame.
        public async Task OpenAsync(Type page, object parameter = null, ViewSizePreference size = ViewSizePreference.UseHalf)
        {
            throw new NotImplementedException();

            //int id = default(int);
            //ApplicationView view = null;
            //var coreview = CoreApplication.CreateNewView();
            //var dispatcher = WindowWrapper.Current().Dispatcher;
            //await coreview.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    var nav = BootStrapper.Current.NavigationServiceFactory(false);
            //    Window.Current.Content = nav.Frame;
            //    nav.Navigate(page, parameter);

            //    view = ApplicationView.GetForCurrentView();
            //    view.Consolidated += (s, e) =>
            //    {
            //        if (!CoreApplication.GetCurrentView().IsMain)
            //            Window.Current.Close();
            //    };
            //    id = view.Id;
            //});
            //if (await ApplicationViewSwitcher.TryShowAsStandaloneAsync(id))
            //{
            //    // Switch to new view
            //    await ApplicationViewSwitcher.SwitchAsync(id);
            //}

            //var coreView = CoreApplication.CreateNewView();
            //ApplicationView view = null;
            //var create = new Action(() =>
            //{
            //    // setup content
            //    var frame = new Frame();
            //    frame.NavigationFailed += (s, e) => { System.Diagnostics.Debugger.Break(); };
            //    frame.Navigate(page, parameter);

            //    // create window
            //    var window = Window.Current;
            //    window.Content = frame;

            //    // setup view/collapse
            //    view = ApplicationView.GetForCurrentView();
            //    Windows.Foundation.TypedEventHandler<ApplicationView, ApplicationViewConsolidatedEventArgs> consolidated = null;
            //    consolidated = new Windows.Foundation.TypedEventHandler<ApplicationView, ApplicationViewConsolidatedEventArgs>((s, e) =>
            //    {
            //        (s as ApplicationView).Consolidated -= consolidated;
            //        if (CoreApplication.GetCurrentView().IsMain)
            //            return;
            //        try { window.Close(); }
            //        finally { CoreApplication.GetCurrentView().CoreWindow.Activate(); }
            //    });
            //    view.Consolidated += consolidated;
            //});

            //// execute create
            //await WindowWrapper.Current().Dispatcher.DispatchAsync(create);

            //// show view
            //if (await ApplicationViewSwitcher.TryShowAsStandaloneAsync(view.Id, size))
            //{
            //    // change focus
            //    await ApplicationViewSwitcher.SwitchAsync(view.Id);
            //}
            //return view.Id;
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

