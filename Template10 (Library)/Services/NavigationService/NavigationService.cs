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
using Template10.Utils;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public partial class NavigationService : INavigationService
    {
        FrameFacade FrameFacadeInternal { get; }
        public FrameFacade FrameFacade => FrameFacadeInternal;
        public bool IsInMainView { get; }
        public Frame Frame => FrameFacade.Frame;
        object LastNavigationParameter { get; set; }
        string LastNavigationType { get; set; }
        public object Content => Frame.Content;

        public DispatcherWrapper Dispatcher => this.GetDispatcherWrapper() as DispatcherWrapper;


        public string NavigationState
        {
            get { return Frame.GetNavigationState(); }
            set { Frame.SetNavigationState(value); }
        }

        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"NavigationService.{caller}");

        #endregion

        public static INavigationService GetForFrame(Frame frame) =>
            WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices).FirstOrDefault(x => x.FrameFacade.Frame.Equals(frame));

        protected internal NavigationService(Frame frame)
        {
            SerializationService = Services.SerializationService.SerializationService.Json;
            IsInMainView = CoreApplication.MainView == CoreApplication.GetCurrentView();
            FrameFacadeInternal = new FrameFacade(this, frame);
            FrameFacadeInternal.Navigating += async (s, e) =>
            {
                if (e.Suspending)
                    return;

                // allow the viewmodel to cancel navigation
                e.Cancel = !(await NavigatingFromAsync(false, e.NavigationMode));
                if (!e.Cancel)
                {
                    await NavigateFromAsync(false);
                }
            };
            FrameFacadeInternal.Navigated += async (s, e) =>
            {
                var parameter = SerializationService.Deserialize(e.Parameter?.ToString());
                var currentContent = FrameFacadeInternal.Frame.Content;
                if (Equals(e.Parameter?.ToString(), SerializationService.Serialize(LastNavigationParameter)))
                    parameter = LastNavigationParameter;
                await this.GetDispatcherWrapper().DispatchAsync(async () =>
                {
                    try
                    {
                        if (currentContent == FrameFacadeInternal.Frame.Content)
                            await NavigateToAsync(e.NavigationMode, parameter, FrameFacadeInternal.Frame.Content);
                    }
                    catch (Exception ex)
                    {
                        DebugWrite($"DispatchAsync/NavigateToAsync {ex.Message}");
                        throw;
                    }
                }, 1);
            };
        }

       

        private INavigable ResolveForPage(Page page)
        {
            if (!(page.DataContext is INavigable) | page.DataContext == null)
            {
                // to support dependency injection, but keeping it optional.
                var viewModel = BootStrapper.Current.ResolveForPage(page, this);
                if ((viewModel != null))
                {
                    page.DataContext = viewModel;
                    return viewModel;
                }
            }
            return page.DataContext as INavigable;
        }

        // before navigate (cancellable)
        async Task<bool> NavigatingFromAsync(bool suspending, NavigationMode mode)
        {
            DebugWrite($"Suspending: {suspending}");

            var page = FrameFacadeInternal.Content as Page;
            if (page != null)
            {
                // force (x:bind) page bindings to update
                XamlUtils.UpdateBindings(page);

                // call navagable override (navigating)
                var dataContext = ResolveForPage(page);
                if (dataContext != null)
                {
                    dataContext.NavigationService = this;
                    dataContext.Dispatcher = this.GetDispatcherWrapper();
                    dataContext.SessionState = BootStrapper.Current.SessionState;
                    var deferral = new DeferralManager();
                    var args = new NavigatingEventArgs(deferral)
                    {
                        NavigationMode = mode,
                        PageType = FrameFacadeInternal.CurrentPageType,
                        Parameter = FrameFacadeInternal.CurrentPageParam,
                        Suspending = suspending,
                    };
                    await deferral.WaitForDeferralsAsync();
                    await dataContext.OnNavigatingFromAsync(args);
                    return !args.Cancel;
                }
            }
            return true;
        }

        // after navigate
        async Task NavigateFromAsync(bool suspending)
        {
            DebugWrite($"Suspending: {suspending}");

            var page = FrameFacadeInternal.Content as Page;
            if (page != null)
            {
                // call viewmodel
                var dataContext = ResolveForPage(page);
                if (dataContext != null)
                {
                    dataContext.NavigationService = this;
                    dataContext.Dispatcher = this.GetDispatcherWrapper();
                    dataContext.SessionState = BootStrapper.Current.SessionState;
                    var pageState = FrameFacadeInternal.PageStateSettingsService(page.GetType()).Values;
                    await dataContext.OnNavigatedFromAsync(pageState, suspending);
                }
            }
        }

        async Task NavigateToAsync(NavigationMode mode, object parameter, object frameContent = null)
        {
            DebugWrite($"Mode: {mode}, Parameter: {parameter} FrameContent: {frameContent}");

            frameContent = frameContent ?? FrameFacadeInternal.Frame.Content;

            LastNavigationParameter = parameter;
            LastNavigationType = frameContent.GetType().FullName;

            var page = frameContent as Page;
            if (page != null)
            {
                if (mode == NavigationMode.New)
                {
                    var pageState = FrameFacadeInternal.PageStateSettingsService(page.GetType()).Values;
                    pageState?.Clear();
                }

                var dataContext = ResolveForPage(page);

                if (dataContext != null)
                {
                    // prepare for state load
                    dataContext.NavigationService = this;
                    dataContext.Dispatcher = this.GetDispatcherWrapper();
                    dataContext.SessionState = BootStrapper.Current.SessionState;
                    var pageState = FrameFacadeInternal.PageStateSettingsService(page.GetType()).Values;
                    await dataContext.OnNavigatedToAsync(parameter, mode, pageState);
                    {
                        // update bindings after NavTo initializes data
                        XamlUtils.InitializeBindings(page);
                        XamlUtils.UpdateBindings(page);
                    }
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
                var newWindow = this.GetWindowWrapper().Window;
                var newAppView = ApplicationView.GetForCurrentView();
                newAppView.Title = title;

                var nav = BootStrapper.Current.NavigationServiceFactory(BootStrapper.BackButton.Ignore, BootStrapper.ExistingContent.Exclude);
                nav.Navigate(page, parameter);
                newWindow.Content = nav.Frame;
                newWindow.Activate();

                await ApplicationViewSwitcher
                    .TryShowAsStandaloneAsync(newAppView.Id, ViewSizePreference.Default, currentView.Id, size);
            });
        }

        public async Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            DebugWrite($"Page: {page}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            // use CurrentPageType/Param instead of LastNavigationType/Parameter to avoid new navigation to the current
            // page in some race conditions.
            if ((page.FullName == CurrentPageType?.FullName) && (parameter == CurrentPageParam))
                return false;

            if ((page.FullName == CurrentPageType?.FullName) && (parameter?.Equals(CurrentPageParam) ?? false))
                return false;


            parameter = SerializationService.Serialize(parameter);

            await Task.CompletedTask;
            return FrameFacadeInternal.Navigate(page, parameter, infoOverride);
        }

        public async void Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            DebugWrite($"Page: {page}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            await NavigateAsync(page, parameter, infoOverride);
        }

        /// <summary>
        /// Navigate<T> allows developers to navigate using a
        /// page key instead of the view type.This is accomplished by
        /// creating a custom Enum and setting up the PageKeys dict
        /// with the Key/Type pairs for your views.The dict is
        /// shared by all NavigationServices and is stored in
        /// the BootStrapper (or Application) of the app.
        /// 
        /// Implementation example:
        /// 
        /// // define your Enum
        /// public Enum Pages { MainPage, DetailPage }
        /// 
        /// // setup the keys dict
        /// var keys = BootStrapper.PageKeys<Views>();
        /// keys.Add(Pages.MainPage, typeof(Views.MainPage));
        /// keys.Add(Pages.DetailPage, typeof(Views.DetailPage));
        /// 
        /// // use Navigate<T>()
        /// NavigationService.Navigate(Pages.MainPage);
        /// </remarks>
        /// <typeparam name="T">T must be the same custom Enum used with BootStrapper.PageKeys()</typeparam>
        public async Task<bool> NavigateAsync<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : struct, IConvertible
        {
            DebugWrite($"Key: {key}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            var keys = BootStrapper.Current.PageKeys<T>();

            if (!keys.ContainsKey(key))
                throw new KeyNotFoundException(key.ToString());

            var page = keys[key];

            return await NavigateAsync(page, parameter, infoOverride);
        }

        public async void Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : struct, IConvertible
        {
            DebugWrite($"Key: {key}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            await NavigateAsync(key, parameter, infoOverride);
        }

        public ISerializationService SerializationService { get; set; }

        public event EventHandler<CancelEventArgs<Type>> BeforeSavingNavigation;
        public async Task SaveNavigationAsync()
        {
            DebugWrite($"Frame: {FrameFacadeInternal.FrameId}");

            if (CurrentPageType == null)
                return;
            var args = new CancelEventArgs<Type>(FrameFacadeInternal.CurrentPageType);
            BeforeSavingNavigation?.Invoke(this, args);
            if (args.Cancel)
                return;

            var state = FrameFacadeInternal.PageStateSettingsService(GetType().ToString());
            if (state == null)
            {
                throw new InvalidOperationException("State container is unexpectedly null");
            }

            state.Write<string>("CurrentPageType", CurrentPageType.AssemblyQualifiedName);
            state.Write<object>("CurrentPageParam", CurrentPageParam);
            state.Write<string>("NavigateState", FrameFacadeInternal?.GetNavigationState());
            await Task.CompletedTask;
        }

        public event TypedEventHandler<Type> AfterRestoreSavedNavigation;
        public async Task<bool> RestoreSavedNavigationAsync()
        {
            DebugWrite($"Frame: {FrameFacadeInternal.FrameId}");

            try
            {
                var state = FrameFacadeInternal.PageStateSettingsService(GetType().ToString());
                if (state == null || !state.Exists("CurrentPageType"))
                {
                    return false;
                }

                FrameFacadeInternal.CurrentPageType = Type.GetType(state.Read<string>("CurrentPageType"));
                FrameFacadeInternal.CurrentPageParam = state.Read<object>("CurrentPageParam");
                FrameFacadeInternal.SetNavigationState(state.Read<string>("NavigateState"));

                await NavigateToAsync(NavigationMode.Refresh, FrameFacadeInternal.CurrentPageParam);
                while (FrameFacadeInternal.Frame.Content == null)
                {
                    Task.Yield().GetAwaiter().GetResult();
                }
                AfterRestoreSavedNavigation?.Invoke(this, FrameFacadeInternal.CurrentPageType);
                return true;
            }
            catch { return false; }
        }

        public void Refresh() { FrameFacadeInternal.Refresh(); }
        public void Refresh(object param) { FrameFacadeInternal.Refresh(param); }

        public void GoBack() { if (FrameFacadeInternal.CanGoBack) FrameFacadeInternal.GoBack(); }

        public bool CanGoBack => FrameFacadeInternal.CanGoBack;

        public void GoForward() { FrameFacadeInternal.GoForward(); }

        public bool CanGoForward => FrameFacadeInternal.CanGoForward;

        public void ClearCache(bool removeCachedPagesInBackStack = false)
        {
            DebugWrite($"Frame: {FrameFacadeInternal.FrameId}");

            int currentSize = FrameFacadeInternal.Frame.CacheSize;

            if (removeCachedPagesInBackStack)
            {
                FrameFacadeInternal.Frame.CacheSize = 0;
            }
            else
            {
                if (FrameFacadeInternal.Frame.BackStackDepth == 0)
                    FrameFacadeInternal.Frame.CacheSize = 1;
                else
                    FrameFacadeInternal.Frame.CacheSize = FrameFacadeInternal.Frame.BackStackDepth;
            }

            FrameFacadeInternal.Frame.CacheSize = currentSize;
        }

        public void ClearHistory() { FrameFacadeInternal.Frame.BackStack.Clear(); }

        public void Resuming() { /* nothing */ }

        public async Task SuspendingAsync()
        {
            DebugWrite($"Frame: {FrameFacadeInternal.FrameId}");

            await SaveNavigationAsync();
            await NavigateFromAsync(true);
        }

        public Type CurrentPageType => FrameFacadeInternal.CurrentPageType;
        public object CurrentPageParam => FrameFacadeInternal.CurrentPageParam;
    }
}

