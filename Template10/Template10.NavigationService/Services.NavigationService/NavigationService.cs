using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common;
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

        Lazy<NavigationLogic> _Navigation;
        private NavigationLogic Navigation => _Navigation.Value;

        Lazy<IViewService> _ViewService;
        private IViewService ViewService => _ViewService.Value;

        Lazy<ISuspensionStateLogic> _Suspension;
        public ISuspensionStateLogic Suspension => _Suspension.Value;

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
            _Navigation = new Lazy<NavigationLogic>(() => new NavigationLogic(this));
            _Suspension = new Lazy<ISuspensionStateLogic>(() => new SuspensionStateLogic(FrameFacadeInternal, this));
            _FrameFacade = new Lazy<IFrameFacade>(() => new FrameFacade(frame, this));
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

            // call oldViewModel.OnNavigatingFromAsync()
            var viewmodelCancels = await Navigation.NavingFromCancelsAsync(oldViewModel, mode, FrameFacade.Content as Page, CurrentPageType, CurrentPageParam, null, page, parameter, false);
            if (viewmodelCancels)
            {
                return false;
            }

            // raise Navigating event
            var eventCancels = RaiseNavigatingCancels(oldPage, parameter, false, mode, page);
            if (eventCancels)
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

            // raise Navigated event
            RaiseNavigated(newPage, parameter, mode);

            // call oldViewModel.OnNavigatedFrom()
            await Navigation.NavedFromAsync(oldViewModel, mode, oldPage, oldPage?.GetType(), oldParameter, newPage, page, parameter, false);

            // call newViewModel.ResolveForPage()
            if (newViewModel == null)
            {
                newViewModel = ResolveViewModelForPage.Invoke(newPage);
                newPage.DataContext = newViewModel;
            }

            // call newTemplate10ViewModel.Properties
            if (newViewModel is INavigable)
            {
                await Navigation.SetupViewModelAsync(this, newViewModel as INavigable);
            }

            // call newViewModel.OnNavigatedToAsync()
            await Navigation.NavedToAsync(newViewModel, mode, oldPage, oldPage?.GetType(), oldParameter, newPage, page, parameter);

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
        public void RaiseNavigated(object page, object parameter, NavigationMode mode)
        {
            var navigatedEventArgs = new NavigatedEventArgs()
            {
                Page = page as Page,
                Parameter = parameter,
                NavigationMode = mode,
                PageType = page?.GetType(),
            };
            RaiseNavigated(navigatedEventArgs);
        }

        public event EventHandler<NavigatingEventArgs> Navigating;
        public void RaiseNavigating(NavigatingEventArgs e)
        {
            Navigating?.Invoke(this, e);
            // for backwards compat
            FrameFacadeInternal.RaiseNavigating(e);
        }

        public bool RaiseNavigatingCancels(object page, object parameter, bool suspending, NavigationMode mode, Type targetType)
        {
            var navigatingDeferral = new Common.DeferralManager();
            var navigatingEventArgs = new NavigatingEventArgs(navigatingDeferral)
            {
                Page = page as Page,
                Parameter = parameter,
                Suspending = suspending,
                NavigationMode = mode,
                TargetPageType = targetType,
                TargetPageParameter = parameter,
            };
            RaiseNavigating(navigatingEventArgs);
            return navigatingEventArgs.Cancel;
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

            var frameState = Suspension.GetFrameState();
            if (frameState == null)
            {
                throw new InvalidOperationException("State container is unexpectedly null");
            }

            frameState.Write<string>("CurrentPageType", CurrentPageType.AssemblyQualifiedName);
            frameState.Write<object>("CurrentPageParam", CurrentPageParam);
            frameState.Write<string>("NavigateState", FrameFacadeInternal.GetNavigationState());

            await Task.CompletedTask;
        }

        public async Task<bool> LoadAsync()
        {
            // load navigation state from settings

            DebugWrite($"Frame: {FrameFacade.FrameId}");

            try
            {
                var frameState = Suspension.GetFrameState();
                if (frameState == null || !frameState.Exists("CurrentPageType"))
                {
                    return false;
                }

                CurrentPageType = frameState.Read<Type>("CurrentPageType");
                CurrentPageParam = frameState.Read<object>("CurrentPageParam");
                FrameFacadeInternal.SetNavigationState(frameState.Read<string>("NavigateState"));

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
                if (newViewModel is INavigable)
                {
                    await Navigation.SetupViewModelAsync(this, newViewModel as INavigable);
                }

                // newNavigatedAwareAsync.OnNavigatedTo
                await Navigation.NavedToAsync(newPage?.DataContext, NavigationMode.New, null, null, null, newPage, CurrentPageType, CurrentPageParam);

                RaiseAfterRestoreSavedNavigation();
                return true;
            }
            catch { return false; }
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

