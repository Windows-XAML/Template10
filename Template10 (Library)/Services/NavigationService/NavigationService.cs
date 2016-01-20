using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Template10.Services.SerializationService;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public partial class NavigationService : INavigationService
    {
        public FrameFacade FrameFacade { get; }
        public Frame Frame => FrameFacade.Frame;
        object LastNavigationParameter { get; set; }
        string LastNavigationType { get; set; }

        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Trace, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"NavigationService.{caller}");

        #endregion

        public static INavigationService GetForFrame(Frame frame) =>
            WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices).FirstOrDefault(x => x.Frame.Equals(frame));

        public DispatcherWrapper Dispatcher => WindowWrapper.Current(this).Dispatcher;

        protected internal NavigationService(Frame frame)
        {
            SerializationService = global::Template10.Services.SerializationService.SerializationService.Json;

            FrameFacade = new FrameFacade(this, frame);
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
            FrameFacade.Navigated += async (s, e) =>
            {
                var parameter = SerializationService.Deserialize(e.Parameter?.ToString());
                await WindowWrapper.Current().Dispatcher.DispatchAsync(() => { NavigateTo(e.NavigationMode, parameter, Frame.Content); }, 1);
            };
        }

        // before navigate (cancellable) 
        bool NavigatingFrom(bool suspending)
        {
            DebugWrite($"Suspending: {suspending}");

            var page = FrameFacade.Content as Page;
            if (page != null)
            {
                // force (x:bind) page bindings to update
                var fields = page.GetType().GetRuntimeFields();
                var bindings = fields.FirstOrDefault(x => x.Name.Equals("Bindings"));
                if (bindings != null)
                {
                    var update = bindings.GetType().GetTypeInfo().GetDeclaredMethod("Update");
                    update?.Invoke(bindings, null);
                }

                // call navagable override (navigating)
                var dataContext = page.DataContext as INavigable;
                if (dataContext != null)
                {
                    var args = new NavigatingEventArgs
                    {
                        NavigationMode = FrameFacade.NavigationModeHint,
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
            DebugWrite($"Suspending: {suspending}");

            var page = FrameFacade.Content as Page;
            if (page != null)
            {
                // call viewmodel
                var dataContext = page.DataContext as INavigable;
                if (dataContext != null)
                {
                    var pageState = FrameFacade.PageStateSettingsService(page.GetType()).Values;
                    await dataContext.OnNavigatedFromAsync(pageState, suspending);
                }
            }
        }

        void NavigateTo(NavigationMode mode, object parameter, object frameContent = null)
        {
            DebugWrite($"Mode: {mode}, Parameter: {parameter} FrameContent: {frameContent}");

            frameContent = frameContent ?? Frame.Content;

            LastNavigationParameter = parameter;
            LastNavigationType = frameContent.GetType().FullName;

            if (mode == NavigationMode.New)
            {
                FrameFacade.ClearFrameState();
            }

            var page = frameContent as Page;
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
                    dataContext.Dispatcher = Common.WindowWrapper.Current(this)?.Dispatcher;
                    dataContext.SessionState = BootStrapper.Current.SessionState;
                    var pageState = FrameFacade.PageStateSettingsService(page.GetType()).Values;
                    dataContext.OnNavigatedTo(parameter, mode, pageState);
                }
            }
        }

        public async Task OpenAsync(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf)
        {
            DebugWrite($"Page: {page}, Parameter: {parameter}, Title: {title}, Size: {size}");

            var currentView = ApplicationView.GetForCurrentView();
            title = title ?? currentView.Title;

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

                await ApplicationViewSwitcher
                    .TryShowAsStandaloneAsync(newAppView.Id, ViewSizePreference.Default, currentView.Id, size);
            });
        }

        public bool Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            DebugWrite($"Page: {page}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (page.FullName.Equals(LastNavigationType))
            {
                if (parameter == LastNavigationParameter)
                    return false;

                if (parameter != null && parameter.Equals(LastNavigationParameter))
                    return false;
            }

            parameter = SerializationService.Serialize(parameter);
            return FrameFacade.Navigate(page, parameter, infoOverride);
        }

        /*
            Navigate<T> allows developers to navigate using a
            page key instead of the view type. This is accomplished by
            creating a custom Enum and setting up the PageKeys dict
            with the Key/Type pairs for your views. The dict is
            shared by all NavigationServices and is stored in
            the BootStrapper (or Application) of the app.

            Implementation example:

            // define your Enum
            public Enum Pages { MainPage, DetailPage }

            // setup the keys dict
            var keys = BootStrapper.PageKeys<Views>();
            keys.Add(Pages.MainPage, typeof(Views.MainPage));
            keys.Add(Pages.DetailPage, typeof(Views.DetailPage));

            // use Navigate<T>()
            NavigationService.Navigate(Pages.MainPage);
        */

        // T must be the same custom Enum used with BootStrapper.PageKeys()
        public bool Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : struct, IConvertible
        {
            DebugWrite($"Key: {key}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            var keys = Common.BootStrapper.Current.PageKeys<T>();

            if (!keys.ContainsKey(key))
                throw new KeyNotFoundException(key.ToString());

            var page = keys[key];

            if (page.FullName.Equals(LastNavigationType))
            {
                if (parameter == LastNavigationParameter)
                    return false;

                if (parameter != null && parameter.Equals(LastNavigationParameter))
                    return false;
            }

            return FrameFacade.Navigate(page, parameter, infoOverride);
        }

        public ISerializationService SerializationService { get; set; }

        public event EventHandler<CancelEventArgs<Type>> BeforeSavingNavigation;
        public void SaveNavigation()
        {
            DebugWrite($"Frame: {FrameFacade.FrameId}");

            if (CurrentPageType == null)
                return;
            var args = new CancelEventArgs<Type>(FrameFacade.CurrentPageType);
            BeforeSavingNavigation?.Invoke(this, args);
            if (args.Cancel)
                return;

            var state = FrameFacade.PageStateSettingsService(GetType());
            if (state == null)
            {
                throw new InvalidOperationException("State container is unexpectedly null");
            }

            state.Write<string>("CurrentPageType", CurrentPageType.AssemblyQualifiedName);
            state.Write<object>("CurrentPageParam", CurrentPageParam);
            state.Write<string>("NavigateState", FrameFacade?.GetNavigationState());
        }

        public event TypedEventHandler<Type> AfterRestoreSavedNavigation;
        public bool RestoreSavedNavigation()
        {
            DebugWrite($"Frame: {FrameFacade.FrameId}");

            try
            {
                var state = FrameFacade.PageStateSettingsService(GetType());
                if (state == null || !state.Exists("CurrentPageType"))
                {
                    return false;
                }

                FrameFacade.CurrentPageType = Type.GetType(state.Read<string>("CurrentPageType"));
                FrameFacade.CurrentPageParam = state.Read<object>("CurrentPageParam");
                FrameFacade.SetNavigationState(state.Read<string>("NavigateState"));
                NavigateTo(NavigationMode.Refresh, FrameFacade.CurrentPageParam);
                while (Frame.Content == null)
                {
                    Task.Yield().GetAwaiter().GetResult();
                }
                AfterRestoreSavedNavigation?.Invoke(this, FrameFacade.CurrentPageType);
                return true;
            }
            catch { return false; }
        }

        public void Refresh() { FrameFacade.Refresh(); }

        public void GoBack() { if (FrameFacade.CanGoBack) FrameFacade.GoBack(); }

        public bool CanGoBack => FrameFacade.CanGoBack;

        public void GoForward() { FrameFacade.GoForward(); }

        public bool CanGoForward => FrameFacade.CanGoForward;

        public void ClearCache(bool removeCachedPagesInBackStack = false)
        {
            DebugWrite($"Frame: {FrameFacade.FrameId}");

            int currentSize = FrameFacade.Frame.CacheSize;

            if (removeCachedPagesInBackStack)
            {
                FrameFacade.Frame.CacheSize = 0;
            }
            else
            {
                if (Frame.BackStackDepth == 0)
                    Frame.CacheSize = 1;
                else
                    Frame.CacheSize = Frame.BackStackDepth;
            }

            FrameFacade.Frame.CacheSize = currentSize;
        }

        public void ClearHistory() { FrameFacade.Frame.BackStack.Clear(); }

        public void Resuming() { /* nothing */ }

        public async Task SuspendingAsync()
        {
            DebugWrite($"Frame: {FrameFacade.FrameId}");

            SaveNavigation();
            await NavigateFromAsync(true);
        }

        public void Show(SettingsFlyout flyout, string parameter = null)
        {
            DebugWrite();

            if (flyout == null)
                throw new ArgumentNullException(nameof(flyout));
            var dataContext = flyout.DataContext as INavigable;
            if (dataContext != null)
            {
                dataContext.OnNavigatedTo(parameter, NavigationMode.New, null);
            }
            flyout.Show();
        }

        public Type CurrentPageType => FrameFacade.CurrentPageType;
        public object CurrentPageParam => FrameFacade.CurrentPageParam;
    }
}

