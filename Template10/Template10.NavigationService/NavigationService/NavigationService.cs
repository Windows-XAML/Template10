using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Portable.Navigation;
using Template10.Portable.State;
using Template10.Services.SerializationService;
using Template10.Services.ViewService;
using Template10.Services.WindowWrapper;
using Template10.Utils;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public partial class NavigationService : INavigationService
    {
        #region Debug

        internal static void DebugWrite(string text = null, LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"NavigationService.{caller}");

        #endregion

        public static INavigationService GetForFrame(Frame frame) => frame.GetNavigationService();

        Lazy<IViewService> _ViewService;
        private IViewService ViewService => _ViewService.Value;

        Lazy<ISerializationService> _SerializationService;
        public ISerializationService SerializationService
        {
            get { return _SerializationService.Value; }
            set { _SerializationService = new Lazy<ISerializationService>(() => value); }
        }

        public IWindowWrapper Window { get; private set; }

        Lazy<IFrameFacade> _FrameFacade;
        public IFrameFacade FrameFacade => _FrameFacade.Value;
        IFrameFacadeInternal FrameFacadeInternal => _FrameFacade.Value as IFrameFacadeInternal;

        public Common.BackButton BackButtonHandling { get; set; }

        public bool IsInMainView { get; }

        public object CurrentPageParam { get; internal set; }

        public Type CurrentPageType { get; internal set; }

        public IDispatcherWrapper Dispatcher => this.GetDispatcherWrapper();

        [Obsolete("Use NavigationService.FrameFacade. This may be made private in future versions.", false)]
        public Frame Frame => FrameFacadeInternal.Frame;

        [Obsolete("Use FrameFacade.Content", false)]
        public object Content => FrameFacade.Content;

        [Obsolete("Use FrameFacade.Get/SetNavigationState()", false)]
        public string NavigationState
        {
            get { return FrameFacadeInternal.GetNavigationState(); }
            set { FrameFacadeInternal.SetNavigationState(value); }
        }

        public static NavigationServiceList Instances { get; } = new NavigationServiceList();

        public NavigationService()
        {
            Window = WindowWrapper.WindowWrapper.Current();
            Instances.Add(this);
            if (Instances.Count == 1)
            {
                Default = this;
            }
        }

        public NavigationService(Frame frame) : this()
        {
            IsInMainView = CoreApplication.MainView == CoreApplication.GetCurrentView();
            _SerializationService = new Lazy<ISerializationService>(() => Services.SerializationService.SerializationService.Json);
            _FrameFacade = new Lazy<IFrameFacade>(() => new FrameFacade(frame, this as INavigationService));
            _ViewService = new Lazy<IViewService>(() => new ViewService.ViewService());
        }

        public Task<IViewLifetimeControl> OpenAsync(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf)
        {
            DebugWrite($"Page: {page}, Parameter: {parameter}, Title: {title}, Size: {size}");

            var frame = new Frame();
            var nav = new NavigationService(frame);
            nav.Navigate(page, parameter);

            return ViewService.OpenAsync(frame, title, size);
        }

        public static INavigationService Default { get; private set; }

        public IViewModelStrategy ViewModelStrategy { get; set; } = new PortableViewModelStrategy { SessionState = Template10.SessionState.Current() };

        #region Navigate methods

        public async Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            DebugWrite($"Page: {page}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            // serialize parameter
            var serializedParameter = default(string);
            try
            {
                serializedParameter = SerializationService.Serialize(parameter);
            }
            catch
            {
                throw new Exception("Parameter cannot be serialized. See https://github.com/Windows-XAML/Template10/wiki/Page-Parameters");
            }

            return await NavigationOrchestratorAsync(page, parameter, NavigationMode.New, () =>
            {
                return FrameFacadeInternal.Navigate(page, serializedParameter, infoOverride);
            });
        }

        public void Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
            => NavigateAsync(page, parameter, infoOverride).ConfigureAwait(true);

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

            var keys = NavigationService.PageKeys<T>();
            if (!keys.TryGetValue(key, out var page))
            {
                throw new KeyNotFoundException(key.ToString());
            }
            return await NavigateAsync(page, parameter, infoOverride).ConfigureAwait(true);
        }

        public void Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : struct, IConvertible => NavigateAsync(key, parameter, infoOverride).ConfigureAwait(true);

        public async Task<IPersistedStateContainer> GetFrameStateAsync()
        {
            return await StateService.StateService.GetAsync($"Frame-{this.FrameFacade.FrameId ?? "NoId"}");
        }

        public async Task<IPersistedStateContainer> GetPageState(Type page)
        {
            if (page == null) return default(IPersistedStateContainer);
            return await StateService.StateService.GetAsync($"Page-{page.ToString()}", $"Frame-{this.FrameFacade.FrameId ?? "NoId"}");
        }

        private async Task<bool> NavigationOrchestratorAsync(Type page, object parameter, NavigationMode mode, Func<bool> navigate)
        {
            DebugWrite($"Page: {page}, Parameter: {parameter}, NavigationMode: {mode}");

            if (page == null) throw new ArgumentNullException(nameof(page));
            if (navigate == null) throw new ArgumentNullException(nameof(navigate));

            // this cannot be used for duplicate navigation, except for refresh
            if ((mode != NavigationMode.Refresh)
                && (page.FullName == CurrentPageType?.GetType().FullName)
                && (parameter?.Equals(CurrentPageParam) ?? false))
            {
                return false;
            }

            // fetch current (which will become old)
            var oldPage = FrameFacade.Content as Page;
            var oldParameter = CurrentPageParam;
            var oldViewModel = oldPage?.DataContext;
            var oldPageState = await GetPageState(oldPage?.GetType());
            var fromInfo = new NavigationInfo(oldPage?.GetType(), oldParameter, oldPageState);

            var toPageState = await GetPageState(page);
            var toInfo = new NavigationInfo(page, parameter, toPageState);

            // call oldViewModel.OnNavigatingFromAsync()
            var viewmodelCancels = await ViewModelStrategy.NavigatingFromAsync((oldViewModel, mode, false), fromInfo, toInfo, this);
            if (viewmodelCancels)
            {
                return false;
            }

            // raise Navigating event
            RaiseNavigatingCancels(parameter, false, mode, toInfo, out var cancel);
            if (cancel)
            {
                return false;
            }

            // invoke navigate (however custom)
            if (navigate.Invoke())
            {
                CurrentPageParam = parameter;
                CurrentPageType = page;
            }
            else
            {
                return false;
            }

            // fetch (current which is now new)
            var newPage = FrameFacade.Content as Page;
            var newViewModel = newPage?.DataContext;

            // note: this has no value now, but it will
            await ViewModelStrategy.NavigatingToAsync((newViewModel, mode), fromInfo, toInfo, this);

            // raise Navigated event
            RaiseNavigated(new NavigatedEventArgs()
            {
                Parameter = parameter,
                NavigationMode = mode,
                PageType = newPage?.GetType(),
            });

            // call oldViewModel.OnNavigatedFrom()
            await ViewModelStrategy.NavigatedFromAsync((oldViewModel, mode, false), fromInfo, toInfo, this);

            // call newViewModel.ResolveForPage()
            if (newViewModel == null)
            {
                newViewModel = ResolveViewModelForPage.Invoke(newPage);
                newPage.DataContext = newViewModel;
            }

            // call newTemplate10ViewModel.Properties
            if (newViewModel is INavigable vm)
            {
                vm.NavigationService = this;
                vm.Dispatcher = this.GetDispatcherWrapper();
            }

            // call newViewModel.OnNavigatedToAsync()
            await ViewModelStrategy.NavigatedToAsync((newViewModel, mode), fromInfo, toInfo, this);

            // finally 
            return true;
        }

        public Func<Page, object> ResolveViewModelForPage { get; set; } = page => null;

        #endregion

        #region events

        public event EventHandler<NavigatedEventArgs> Navigated;
        public void RaiseNavigated(NavigatedEventArgs e)
        {
            Navigated?.Invoke(this, e);
            // for backwards compat
            FrameFacadeInternal.RaiseNavigated(e);
        }

        public event EventHandler<NavigatingEventArgs> Navigating;
        public void RaiseNavigatingCancels(object parameter, bool suspending, NavigationMode mode, NavigationInfo toInfo, out bool cancel)
        {
            var navigatingDeferral = new Common.DeferralManager();
            var navigatingEventArgs = new NavigatingEventArgs(navigatingDeferral)
            {
                Parameter = parameter,
                Suspending = suspending,
                NavigationMode = mode,
                TargetPageType = toInfo.PageType,
                TargetPageParameter = toInfo.Parameter,
            };
            Navigating?.Invoke(this, navigatingEventArgs);
            // for backwards compat
            FrameFacadeInternal.RaiseNavigating(e);
            cancel = navigatingEventArgs.Cancel;
        }

        public event EventHandler<HandledEventArgs> BackRequested;
        public void RaiseBackRequested(HandledEventArgs args)
        {
            BackRequested?.Invoke(this, args);
            // for backwards compat
            FrameFacadeInternal.RaiseBackRequested(args);
        }

        public event EventHandler<HandledEventArgs> ForwardRequested;
        public void RaiseForwardRequested(HandledEventArgs args)
        {
            ForwardRequested?.Invoke(this, args);
            // for backwards compat
            FrameFacadeInternal.RaiseForwardRequested(args);
        }

        public event EventHandler<CancelEventArgs<Type>> BeforeSavingNavigation;
        bool RaiseBeforeSavingNavigation()
        {
            var args = new CancelEventArgs<Type>(CurrentPageType);
            BeforeSavingNavigation?.Invoke(this, args);
            return args.Cancel;
        }

        public event TypedEventHandler<Type> AfterRestoreSavedNavigation;
        void RaiseAfterRestoreSavedNavigation() => AfterRestoreSavedNavigation?.Invoke(this, CurrentPageType);

        #endregion

        #region Save/Load Navigation methods

        public async Task SaveAsync()
        {
            // save navigation state into settings

            DebugWrite($"Frame: {FrameFacade.FrameId}");

            if (CurrentPageType == null)
                return;
            if (RaiseBeforeSavingNavigation())
                return;

            var frameState = await GetFrameStateAsync();
            await frameState.SetValueAsync<string>("CurrentPageType", CurrentPageType.AssemblyQualifiedName);
            await frameState.SetValueAsync<object>("CurrentPageParam", CurrentPageParam);
            await frameState.SetValueAsync<string>("NavigateState", FrameFacadeInternal.GetNavigationState());

            await Task.CompletedTask;
        }

        public async Task<bool> LoadAsync()
        {
            // load navigation state from settings

            DebugWrite($"Frame: {FrameFacade.FrameId}");

            try
            {
                var frameState = await GetFrameStateAsync();

                var currentPageType = await frameState.TryGetValueAsync<Type>("CurrentPageType");
                if (currentPageType.Success)
                {
                    CurrentPageType = currentPageType.Value;
                }

                var currentPageParam = await frameState.TryGetValueAsync<object>("CurrentPageParam");
                if (currentPageParam.Success)
                {
                    CurrentPageParam = currentPageParam.Value;
                }

                var state = await frameState.TryGetValueAsync("NavigateState");
                if (state.Success && !string.IsNullOrEmpty(state.Value?.ToString()))
                {
                    FrameFacadeInternal.SetNavigationState(state.Value.ToString());
                }

                while (FrameFacade.Content == null)
                {
                    await Task.Delay(1);
                }

                var newPage = FrameFacade.Content as Page;
                var newViewModel = newPage?.DataContext;

                if (newPage?.GetType() != CurrentPageType)
                {
                    // failed to load
                    return false;
                }

                // newTemplate10ViewModel.Properties
                if (newViewModel is INavigable vm)
                {
                    vm.NavigationService = this;
                    vm.Dispatcher = this.GetDispatcherWrapper();
                }

                // newNavigatedAwareAsync.OnNavigatedTo
                var toInfo = new NavigationInfo(CurrentPageType, FrameFacade.BackStack.Last().Parameter, await GetPageState(CurrentPageType));
                await ViewModelStrategy.NavigatedToAsync((newPage?.DataContext, NavigationMode.New), null, toInfo, this);

                return true;
            }
            catch { return false; }
            finally { RaiseAfterRestoreSavedNavigation(); }
        }

        #endregion

        #region Refresh methods

        public void Refresh() => RefreshAsync().ConfigureAwait(true);

        public void Refresh(object param) => RefreshAsync(param).ConfigureAwait(true);

        public async Task<bool> RefreshAsync()
        {
            return await NavigationOrchestratorAsync(CurrentPageType, CurrentPageParam, NavigationMode.Refresh, () =>
            {
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
                FrameFacadeInternal.SetNavigationState(FrameFacadeInternal.GetNavigationState());
                return true;
            });
        }

        public async Task<bool> RefreshAsync(object param)
        {
            var history = FrameFacade.BackStack.Last();
            return await NavigationOrchestratorAsync(CurrentPageType, param, NavigationMode.Refresh, () =>
            {
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
                FrameFacadeInternal.SetNavigationState(FrameFacadeInternal.GetNavigationState());
                return true;
            });
        }

        #endregion

        #region GoBack methods

        public bool CanGoBack => FrameFacade.CanGoBack;

        public void GoBack(NavigationTransitionInfo infoOverride = null) => GoBackAsync(infoOverride).ConfigureAwait(true);

        public async Task<bool> GoBackAsync(NavigationTransitionInfo infoOverride = null)
        {
            if (!CanGoBack)
            {
                return false;
            }
            var previous = FrameFacade.BackStack.LastOrDefault();
            var parameter = SerializationService.Deserialize(previous.Parameter?.ToString());
            return await NavigationOrchestratorAsync(previous.SourcePageType, parameter, NavigationMode.Back, () =>
            {
                FrameFacadeInternal.GoBack(infoOverride);
                return true;
            });
        }

        #endregion

        #region GoForward methods

        public bool CanGoForward => FrameFacade.CanGoForward;

        public void GoForward() => GoForwardAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task<bool> GoForwardAsync()
        {
            if (!FrameFacade.CanGoForward)
            {
                return false;
            }
            var next = FrameFacade.ForwardStack.FirstOrDefault();
            var parameter = SerializationService.Deserialize(next.Parameter?.ToString());
            return await NavigationOrchestratorAsync(next.SourcePageType, parameter, NavigationMode.Forward, () =>
            {
                FrameFacadeInternal.GoForward();
                return true;
            });
        }

        #endregion

        [Obsolete("Call FrameFacade.ClearCache(). This may be private in future versions.", false)]
        public void ClearCache(bool removeCachedPagesInBackStack = false) => FrameFacadeInternal.ClearCache(removeCachedPagesInBackStack);

        public void ClearHistory() => FrameFacade.BackStack.Clear();

        public void Resuming() { /* nothing */ }

        public async Task SuspendingAsync()
        {
            DebugWrite($"Frame: {FrameFacade.FrameId}");

            string CacheDateKey = "Setting-Cache-Date";
            Suspension.GetFrameState().Write(CacheDateKey, DateTime.Now.ToString());

            var dispatcher = this.GetDispatcherWrapper();
            await dispatcher.DispatchAsync(async () =>
            {
                var page = FrameFacade.Content as Page;
                await Navigation.NavedFromAsync(page?.DataContext, NavigationMode.New, page, CurrentPageType, CurrentPageParam, null, null, null, true);
            });
        }

        private static object _PageKeys;
        public static Dictionary<T, Type> PageKeys<T>()
            where T : struct, IConvertible
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            if (_PageKeys != null && _PageKeys is Dictionary<T, Type>)
            {
                return _PageKeys as Dictionary<T, Type>;
            }
            return (_PageKeys = new Dictionary<T, Type>()) as Dictionary<T, Type>;
        }

    }
}

