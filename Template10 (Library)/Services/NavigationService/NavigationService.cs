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
using Template10.Services.ViewService;
using Template10.Utils;
using System.Diagnostics;
using Template10.Services.SettingsService;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public partial class NavigationService : INavigationService
    {
        public static INavigationService GetForFrame(Frame frame) =>
            WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices).FirstOrDefault(x => x.Frame.Equals(frame));

        public BootStrapper.BackButton BackButtonHandling { get; set; }

        private readonly IViewService viewService = new ViewService.ViewService();

        public FrameFacade FrameFacade { get; private set; }

        [Obsolete("Use NavigationService.FrameFacade. This may be made private in future versions.", false)]
        public Frame Frame => FrameFacade.Frame;

        public bool IsInMainView { get; }

        public object CurrentPageParam { get; internal set; }

        public Type CurrentPageType { get; internal set; }

        [Obsolete("Use FrameFacade.Content", true)]
        public object Content => FrameFacade.Content;

        public IDispatcherWrapper Dispatcher => this.GetDispatcherWrapper();

        [Obsolete("Use FrameFacade.Get/SetNavigationState()", true)]
        public string NavigationState { get { return FrameFacade.GetNavigationState(); } set { FrameFacade.SetNavigationState(value); } }

        public ISerializationService SerializationService { get; set; } = Services.SerializationService.SerializationService.Json;

        protected internal NavigationService(Frame frame)
        {
            IsInMainView = CoreApplication.MainView == CoreApplication.GetCurrentView();

            FrameFacade = Navigation.FrameFacade = new FrameFacade(frame, this);
            Navigation.NavigationService = this;

            Suspension = new SuspensionStateLogic(FrameFacade);
            Suspension.NavigationService = this;

            BackRequested += async (s, e) =>
            {
                if (BackButtonHandling == BootStrapper.BackButton.Attach && !e.Handled)
                {
                    await GoBackAsync();
                }
            };
            ForwardRequested += async (s, e) =>
            {
                if (!e.Handled)
                {
                    await GoForwardAsync();
                }
            };
        }

        #region Debug

        internal static void DebugWrite(string text = null, LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"NavigationService.{caller}");

        #endregion

        public Task<ViewLifetimeControl> OpenAsync(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf)
        {
            DebugWrite($"Page: {page}, Parameter: {parameter}, Title: {title}, Size: {size}");
            return viewService.OpenAsync(page, parameter, title, size);
        }

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

            return await InternalNavigateAsync(page, parameter, NavigationMode.New, () =>
            {
                return FrameFacade.Navigate(page, serializedParameter, infoOverride);
            });
        }

        public void Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            DebugWrite($"Page: {page}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            NavigateAsync(page, parameter, infoOverride).ConfigureAwait(true);
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

            Type page;
            var keys = BootStrapper.Current.PageKeys<T>();
            if (!keys.TryGetValue(key, out page))
            {
                throw new KeyNotFoundException(key.ToString());
            }
            return await NavigateAsync(page, parameter, infoOverride).ConfigureAwait(true);
        }

        public void Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : struct, IConvertible
        {
            DebugWrite($"Key: {key}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            NavigateAsync(key, parameter, infoOverride).ConfigureAwait(true);
        }

        private async Task<bool> InternalNavigateAsync(Type page, object parameter, NavigationMode mode, Func<bool> navigate)
        {
            DebugWrite();

            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            // if the identical page+parameter, then don't go
            if ((page.FullName == CurrentPageType?.GetType().FullName) && (parameter?.Equals(CurrentPageParam) ?? false))
            {
                return false;
            }

            // fetch
            var oldPage = FrameFacade.Content as Page;
            var oldViewModel = oldPage?.DataContext;

            // oldViewModel.OnNavigatingFromAsync()
            var oldNavigatingAwareAsync = oldViewModel as INavigatingAwareAsync;
            if (oldNavigatingAwareAsync != null)
            {
                var canNavigate = await Navigation.NavingFromAsync(oldNavigatingAwareAsync, oldPage, CurrentPageParam, false, mode, page, parameter);
                if (!canNavigate)
                {
                    return false;
                }
            }

            // raise Navigating event
            var navigatingDeferral = new DeferralManager();
            var navigatingEventArgs = new NavigatingEventArgs(navigatingDeferral)
            {
                Page = oldPage,
                Parameter = CurrentPageParam,
                Suspending = false,
                NavigationMode = mode,
                TargetPageType = page,
                TargetPageParameter = parameter,
            };
            RaiseNavigating(navigatingEventArgs);
            await navigatingDeferral.WaitForDeferralsAsync();
            if (navigatingEventArgs.Cancel)
            {
                return false;
            }

            // navigate
            if (navigate.Invoke())
            {
                CurrentPageParam = parameter;
                CurrentPageType = page;
            }
            else
            {
                return false;
            }

            // fetch
            var newPage = FrameFacade.Content as Page;
            var newViewModel = newPage?.DataContext;

            // raise Navigated event
            var navigatedEventArgs = new NavigatedEventArgs()
            {
                Page = newPage,
                Parameter = parameter,
                NavigationMode = mode,
                PageType = newPage?.GetType(),
            };
            RaiseNavigated(navigatedEventArgs);

            // oldViewModel.OnNavigatedFrom()
            var oldNavigatedAwareAsync = oldViewModel as INavigatedAwareAsync;
            if (oldNavigatedAwareAsync != null)
            {
                await Navigation.NavedFromAsync(oldPage, oldNavigatedAwareAsync, false);
            }

            // newViewModel.ResolveForPage()
            if (newViewModel == null)
            {
                newPage.DataContext = BootStrapper.Current.ResolveForPage(newPage, this);
            }

            // newTemplate10ViewModel.Properties
            var newTemplate10ViewModel = newViewModel as ITemplate10ViewModel;
            if (newTemplate10ViewModel != null)
            {
                Navigation.SetupViewModel(this, newTemplate10ViewModel);
            }

            // newViewModel.OnNavigatedToAsync()
            var newNavigatedAwareAsync = newViewModel as INavigatedAwareAsync;
            if (newNavigatedAwareAsync != null)
            {
                await Navigation.NavedToAsync(parameter, mode, newPage, newNavigatedAwareAsync);
            }

            // finally 
            return true;
        }

        #endregion

        #region events

        public event EventHandler<NavigatedEventArgs> Navigated;
        public void RaiseNavigated(NavigatedEventArgs e)
        {
            Navigated?.Invoke(this, e);
            // for backwards compat
            FrameFacade.RaiseNavigated(e);
        }

        public event EventHandler<NavigatingEventArgs> Navigating;
        public void RaiseNavigating(NavigatingEventArgs e)
        {
            Navigating?.Invoke(this, e);
            // for backwards compat
            FrameFacade.RaiseNavigating(e);
        }

        public event EventHandler<HandledEventArgs> BackRequested;
        public void RaiseBackRequested(HandledEventArgs args)
        {
            BackRequested?.Invoke(this, args);
            // for backwards compat
            FrameFacade.RaiseBackRequested(args);
        }

        public event EventHandler<HandledEventArgs> ForwardRequested;
        public void RaiseForwardRequested(HandledEventArgs args)
        {
            ForwardRequested?.Invoke(this, args);
            // for backwards compat
            FrameFacade.RaiseForwardRequested(args);
        }

        public event EventHandler<CancelEventArgs<Type>> BeforeSavingNavigation;
        bool RaiseBeforeSavingNavigation()
        {
            var args = new CancelEventArgs<Type>(CurrentPageType);
            BeforeSavingNavigation?.Invoke(this, args);
            return args.Cancel;
        }

        public event TypedEventHandler<Type> AfterRestoreSavedNavigation;
        void RaiseAfterRestoreSavedNavigation()
        {
            AfterRestoreSavedNavigation?.Invoke(this, CurrentPageType);
        }

        #endregion

        #region Save/Load Navigation methods

        public async Task SaveAsync()
        {
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
            frameState.Write<string>("NavigateState", FrameFacade.GetNavigationState());

            await Task.CompletedTask;
        }

        public async Task<bool> LoadAsync()
        {
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
                FrameFacade.SetNavigationState(frameState.Read<string>("NavigateState"));

                while (FrameFacade.Content == null)
                {
                    await Task.Delay(1);
                }

                var newPage = FrameFacade.Content as Page;
                var newViewModel = newPage?.DataContext;

                // newTemplate10ViewModel.Properties
                var newTemplate10ViewModel = newViewModel as ITemplate10ViewModel;
                if (newTemplate10ViewModel != null)
                {
                    Navigation.SetupViewModel(this, newTemplate10ViewModel);
                }

                // newNavigatedAwareAsync.OnNavigatedTo
                var newNavigatedAwareAsync = newPage?.DataContext as INavigatedAwareAsync;
                if (newViewModel != null)
                {
                    await Navigation.NavedToAsync(CurrentPageParam, NavigationMode.Refresh, newPage, newNavigatedAwareAsync);
                }

                RaiseAfterRestoreSavedNavigation();
                return true;
            }
            catch { return false; }
        }

        #endregion

        #region Refresh methods

        public void Refresh()
        {
            RefreshAsync().ConfigureAwait(true);
        }

        public void Refresh(object param)
        {
            RefreshAsync(param).ConfigureAwait(true);
        }

        public async Task<bool> RefreshAsync()
        {
            return await InternalNavigateAsync(CurrentPageType, CurrentPageParam, NavigationMode.Refresh, () =>
            {
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
                FrameFacade.SetNavigationState(FrameFacade.GetNavigationState());
                return true;
            });
        }

        public async Task<bool> RefreshAsync(object param)
        {
            var history = FrameFacade.BackStack.Last();
            return await InternalNavigateAsync(CurrentPageType, param, NavigationMode.Refresh, () =>
            {
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
                FrameFacade.SetNavigationState(FrameFacade.GetNavigationState());
                return true;
            });
        }

        #endregion

        #region GoBack methods

        public void GoBack(NavigationTransitionInfo infoOverride = null)
        {
            GoBackAsync(infoOverride).ConfigureAwait(true);
        }

        public async Task<bool> GoBackAsync(NavigationTransitionInfo infoOverride = null)
        {
            if (!CanGoBack)
            {
                return false;
            }
            var previous = FrameFacade.BackStack.LastOrDefault();
            return await InternalNavigateAsync(previous.SourcePageType, previous.Parameter, NavigationMode.Back, () =>
            {
                FrameFacade.GoBack(infoOverride);
                return true;
            });
        }

        public bool CanGoBack => FrameFacade.CanGoBack;

        #endregion

        #region GoForward methods

        public void GoForward()
        {
            GoForwardAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<bool> GoForwardAsync()
        {
            if (!FrameFacade.CanGoForward)
            {
                return false;
            }
            var next = FrameFacade.ForwardStack.FirstOrDefault();
            return await InternalNavigateAsync(next.SourcePageType, next.Parameter, NavigationMode.Forward, () =>
            {
                FrameFacade.GoForward();
                return true;
            });
        }

        public bool CanGoForward => FrameFacade.CanGoForward;

        #endregion

        [Obsolete("Call FrameFacade.ClearCache(). This may be private in future versions.", false)]
        public void ClearCache(bool removeCachedPagesInBackStack = false) => FrameFacade.ClearCache(removeCachedPagesInBackStack);

        private NavigationLogic Navigation { get; } = new NavigationLogic();

        public SuspensionStateLogic Suspension { get; internal set; }

        public void ClearHistory() { FrameFacade.BackStack.Clear(); }

        public void Resuming() { /* nothing */ }

        public async Task SuspendingAsync()
        {
            DebugWrite($"Frame: {FrameFacade.FrameId}");

            await SaveAsync();

            var page = FrameFacade.Content as Page;
            var viewmodel = page?.DataContext as INavigatedAwareAsync;
            if (viewmodel != null)
            {
                await Navigation.NavedFromAsync(page, viewmodel, true);
            }
        }
    }

    public class NavigationLogic
    {
        internal NavigationLogic()
        {

        }

        public INavigationService NavigationService { get; set; }
        public FrameFacade FrameFacade { get; set; }

        private Windows.Foundation.Collections.IPropertySet State(Page page)
        {
            return NavigationService.Suspension.GetPageState(page.GetType()).Values;
        }

        public void SetupViewModel(INavigationService service, ITemplate10ViewModel viewmodel)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (viewmodel == null)
            {
                throw new ArgumentNullException(nameof(viewmodel));
            }
            viewmodel.NavigationService = service;
            viewmodel.Dispatcher = service.GetDispatcherWrapper();
            viewmodel.SessionState = BootStrapper.Current.SessionState;
        }

        public async Task NavedFromAsync(Page page, INavigatedAwareAsync viewmodel, bool suspending)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (viewmodel == null)
            {
                throw new ArgumentNullException(nameof(viewmodel));
            }
            await viewmodel.OnNavigatedFromAsync(State(page), suspending);
        }

        public async Task NavedToAsync(object parameter, NavigationMode mode, Page page, INavigatedAwareAsync viewmodel)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (viewmodel == null)
            {
                throw new ArgumentNullException(nameof(viewmodel));
            }
            if (mode == NavigationMode.New)
            {
                State(page).Clear();
            }
            await viewmodel.OnNavigatedToAsync(parameter, mode, State(page));
            XamlUtils.InitializeBindings(page);
            XamlUtils.UpdateBindings(page);
        }

        public async Task<bool> NavingFromAsync(INavigatingAwareAsync viewmodel, Page currentPage, object currentParameter, bool suspending, NavigationMode navigationMode, Type targetPageType, object targetParameter)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (viewmodel == null)
            {
                throw new ArgumentNullException(nameof(viewmodel));
            }
            var deferral = new DeferralManager();
            var navigatingEventArgs = new NavigatingEventArgs(deferral)
            {
                Page = currentPage,
                Parameter = currentParameter,
                Suspending = suspending,
                NavigationMode = navigationMode,
                TargetPageType = targetPageType,
                TargetPageParameter = targetParameter,
            };
            try
            {
                await viewmodel.OnNavigatingFromAsync(navigatingEventArgs);
                await deferral.WaitForDeferralsAsync();
            }
            catch
            {
                Debugger.Break();
            }
            return !navigatingEventArgs.Cancel;
        }
    }

    public class SuspensionStateLogic
    {
        FrameFacade frame;
        public INavigationService NavigationService { get; set; }

        internal SuspensionStateLogic(FrameFacade frame)
        {
            this.frame = frame;
        }

        public ISettingsService GetFrameState()
        {
            Services.NavigationService.NavigationService.DebugWrite();

            var container = $"{frame.FrameId}-Frame-SuspensionState";
            return SettingsService.SettingsService.Create(SettingsStrategies.Local, container, true);
        }

        public void ClearFrameState()
        {
            Services.NavigationService.NavigationService.DebugWrite();

            GetFrameState().Clear(true);
        }

        public ISettingsService GetPageState(Type page, int backStackDepth = -1)
        {
            Services.NavigationService.NavigationService.DebugWrite($"page:{page} backStackDepth:{backStackDepth}");

            var folder = $"{page}-{backStackDepth}-Page-SuspensionState";
            return GetPageState(folder);
        }

        public ISettingsService GetPageState(string folder)
        {
            Services.NavigationService.NavigationService.DebugWrite($"folder:{folder}");

            return GetFrameState().Open(folder, true);
        }

        public void ClearPageState(Type type, int backStackDepth = -1)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            GetPageState(type, backStackDepth).Clear();
        }
    }
}

