using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Portable.Navigation;
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
    public partial class NavigationService : INavigationServiceInternal
    {
        #region Debug

        internal static void DebugWrite(string text = null, LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"NavigationService.{caller}");

        #endregion

        Lazy<IViewService> _ViewService;
        private IViewService ViewService => _ViewService.Value;

        public IWindowWrapper Window { get; private set; }

        public ITemplate10Frame FrameFacade { get; private set; }
        ITemplate10FrameInternal FrameFacadeInternal => FrameFacade as ITemplate10FrameInternal;

        public BackButton BackButtonHandling { get; set; }

        public object CurrentPageParam { get; internal set; }

        public Type CurrentPageType { get; internal set; }

        internal NavigationService()
        {
            Window = WindowWrapperManager.Current();
        }

        internal NavigationService(Frame frame) : this()
        {
            FrameFacade = Template10FrameFactory.Create(frame, this as INavigationService);
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

        #region Navigate methods

        public async Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            DebugWrite($"Page: {page}, Parameter: {parameter ?? "null"}, NavigationTransitionInfo: {infoOverride}");

            // serialize parameter
            var serializedParameter = default(string);
            try
            {
                serializedParameter = Settings.SerializationStrategy.Serialize(parameter) ?? string.Empty;
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

            var keys = NavigationServiceHelper.PageKeys<T>();
            if (!keys.TryGetValue(key, out var page))
            {
                throw new KeyNotFoundException(key.ToString());
            }
            return await NavigateAsync(page, parameter, infoOverride).ConfigureAwait(true);
        }

        public void Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : struct, IConvertible => NavigateAsync(key, parameter, infoOverride).ConfigureAwait(true);

        static SemaphoreSlim navigationOrchestratorAsyncSemaphore = new SemaphoreSlim(1, 1);
        private async Task<bool> NavigationOrchestratorAsync(Type page, object parameter, NavigationMode mode, Func<bool> navigate)
        {
            if (!await navigationOrchestratorAsyncSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
            {
                throw new TimeoutException("Semaphore wait ellapsed");
            }

            // using (var locker = await LockAsync.Create(navigationLock))
            try
            {
                DebugWrite($"Page: {page}, Parameter: {parameter}, NavigationMode: {mode}");

                if (page == null) throw new ArgumentNullException(nameof(page));
                if (navigate == null) throw new ArgumentNullException(nameof(navigate));

                // this cannot be used for duplicate navigation, except for refresh
                if (!Equals(mode, NavigationMode.Refresh))
                {
                    if (Equals(page, CurrentPageType))
                    {
                        if (Equals(parameter, CurrentPageParam))
                        {
                            return false;
                        }
                    }
                }

                // fetch current (which will become old)
                var oldPage = FrameFacadeInternal.Content as Page;
                var oldParameter = CurrentPageParam;
                var oldViewModel = oldPage?.DataContext;
                var oldPageName = oldPage?.GetType().ToString();
                var oldPageState = await FrameFacadeInternal.GetPageStateAsync(oldPage?.GetType());
                var from = new NavigationInfo(oldPage?.GetType(), oldParameter, oldPageState);

                var newPageName = page?.ToString();
                var newPageState = await FrameFacadeInternal.GetPageStateAsync(newPageName);
                var to = new NavigationInfo(page, parameter, newPageState);

                // call oldViewModel.OnNavigatingFromAsync()
                var cancelled = await Settings.ViewModelActionStrategy.NavigatingFromAsync((oldViewModel, mode, false), from, to, this);
                if (cancelled)
                {
                    return false;
                }

                // raise Navigating event
                RaiseNavigatingCancels(parameter, false, mode, to, out var cancel);
                if (cancel)
                {
                    return false;
                }

                // try to resolve the view-model before navigation
                var newViewModel = default(object);
                switch (mode)
                {
                    case NavigationMode.New:
                    case NavigationMode.Refresh:
                        var strategy = Settings.ViewModelResolutionStrategy;
                        newViewModel = await strategy.ResolveViewModel(page);
                        if (newViewModel != null)
                        {
                            await Settings.ViewModelActionStrategy.NavigatingToAsync((newViewModel, mode, false), from, to, this);
                        }
                        break;
                }

                // navigate
                var newPage = default(Page);
                if (navigate.Invoke())
                {
                    if ((newPage = FrameFacadeInternal.Content as Page) == null)
                    {
                        return false;
                    }
                    CurrentPageParam = parameter;
                    CurrentPageType = page;
                }
                else
                {
                    return false;
                }

                // fetch current (which is now new)
                if (newViewModel != null)
                {
                    newPage.DataContext = newViewModel;
                }
                else if ((newViewModel = newPage?.DataContext) != null)
                {
                    await Settings.ViewModelActionStrategy.NavigatingToAsync((newViewModel, mode, false), from, to, this);
                }

                // raise Navigated event
                RaiseNavigated(new NavigatedEventArgs()
                {
                    Parameter = parameter,
                    NavigationMode = mode,
                    PageType = newPage?.GetType(),
                });

                // call oldViewModel.OnNavigatedFrom()
                await Settings.ViewModelActionStrategy.NavigatedFromAsync((oldViewModel, mode, false), from, to, this);

                // call newTemplate10ViewModel.Properties
                if (newViewModel is ITemplate10ViewModel vm)
                {
                    vm.NavigationService = this;
                }

                // call newViewModel.OnNavigatedToAsync()
                await Settings.ViewModelActionStrategy.NavigatedToAsync((newViewModel, mode, false), from, to, this);

                // finally, all-good 
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                navigationOrchestratorAsyncSemaphore.Release();
            }
        }

        #endregion

        #region events

        public event EventHandler<NavigatedEventArgs> Navigated;
        public void RaiseNavigated(NavigatedEventArgs e)
        {
            Navigated?.Invoke(this, e);
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
            cancel = navigatingEventArgs.Cancel;
        }

        public event EventHandler<HandledEventArgs> BackRequested;
        void INavigationServiceInternal.RaiseBackRequested(HandledEventArgs args)
        {
            BackRequested?.Invoke(this, args);
        }

        public event EventHandler<HandledEventArgs> ForwardRequested;
        void INavigationServiceInternal.RaiseForwardRequested(HandledEventArgs args)
        {
            ForwardRequested?.Invoke(this, args);
        }

        public event EventHandler<CancelEventArgs<Type>> BeforeSavingNavigation;
        bool RaiseBeforeSavingNavigation()
        {
            var args = new CancelEventArgs<Type>(CurrentPageType);
            BeforeSavingNavigation?.Invoke(this, args);
            return args.Cancel;
        }

        TypedEventHandler<Type> afterRestoreSavedNavigation;
        void RaiseAfterRestoreSavedNavigation() => afterRestoreSavedNavigation?.Invoke(this, CurrentPageType);
        event TypedEventHandler<Type> INavigationServiceInternal.AfterRestoreSavedNavigation
        {
            add => afterRestoreSavedNavigation += value;
            remove => afterRestoreSavedNavigation -= value;
        }

        #endregion

        #region Save/Load Navigation methods

        async Task INavigationServiceInternal.SaveAsync(bool navigateFrom)
        {
            // save navigation state into settings

            DebugWrite($"Frame: {FrameFacadeInternal.FrameId}");

            if (CurrentPageType == null) return;
            if (RaiseBeforeSavingNavigation()) return;

            if (navigateFrom)
            {
                var vm = (FrameFacadeInternal.Content as Page)?.DataContext;
                var fromInfo = new NavigationInfo(CurrentPageType, CurrentPageParam, await FrameFacadeInternal.GetPageStateAsync(CurrentPageType));
                await Settings.ViewModelActionStrategy.NavigatingFromAsync((vm, NavigationMode.New, true), fromInfo, null, this);
                await Settings.ViewModelActionStrategy.NavigatedFromAsync((vm, NavigationMode.New, true), fromInfo, null, this);
            }

            var frameState = await FrameFacadeInternal.GetFrameStateAsync();
            var navigationState = FrameFacadeInternal.GetNavigationState();
            await frameState.SetNavigationStateAsync(navigationState);

            var writtenState = await frameState.TryGetNavigationStateAsync();
            DebugWrite($"navigationState:{navigationState} writtenState:{writtenState}");
            Debug.Assert(navigationState.Equals(writtenState.Value), "Checking frame nav state save");

            await Task.CompletedTask;
        }

        async Task<bool> INavigationServiceInternal.LoadAsync(bool navigateTo)
        {
            DebugWrite($"Frame: {FrameFacadeInternal.FrameId}");

            try
            {
                // load navigation state from settings
                var frameState = await FrameFacadeInternal.GetFrameStateAsync();
                {
                    var state = await frameState.TryGetNavigationStateAsync();
                    DebugWrite($"After TryGetNavigationStateAsync; state: {state.Value ?? "Null"}");
                    if (state.Success && !string.IsNullOrEmpty(state.Value?.ToString()))
                    {
                        FrameFacadeInternal.SetNavigationState(state.Value.ToString());
                        await frameState.SetNavigationStateAsync(string.Empty);
                    }
                    else
                    {
                        return false;
                    }
                }

                // is there a page there?
                var newPage = FrameFacadeInternal.Content as Page;
                if (newPage == null)
                {
                    return false;
                }

                // is there a view-model there?
                var viewModel = newPage?.DataContext;
                if (viewModel is ITemplate10ViewModel vm && vm != null)
                {
                    vm.NavigationService = this;
                }


                // NavigatingToAsync/NavigatedToAsync
                if (navigateTo)
                {
                    CurrentPageType = FrameFacade.Content?.GetType();
                    var toInfo = new NavigationInfo(CurrentPageType, null, await FrameFacadeInternal.GetPageStateAsync(CurrentPageType));
                    await Settings.ViewModelActionStrategy.NavigatingToAsync((viewModel, NavigationMode.New, true), null, toInfo, this);
                    await Settings.ViewModelActionStrategy.NavigatedToAsync((viewModel, NavigationMode.New, true), null, toInfo, this);
                }

                // tell them it worked
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
            var history = FrameFacadeInternal.BackStack.Last();
            return await NavigationOrchestratorAsync(CurrentPageType, param, NavigationMode.Refresh, () =>
            {
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
                FrameFacadeInternal.SetNavigationState(FrameFacadeInternal.GetNavigationState());
                return true;
            });
        }

        #endregion

        #region GoBack methods

        public bool CanGoBack => FrameFacadeInternal.CanGoBack;

        public void GoBack(NavigationTransitionInfo infoOverride = null) => GoBackAsync(infoOverride).ConfigureAwait(true);

        public async Task<bool> GoBackAsync(NavigationTransitionInfo infoOverride = null)
        {
            if (!CanGoBack)
            {
                return true;
            }
            var previous = FrameFacadeInternal.BackStack.LastOrDefault();
            var parameter = Settings.SerializationStrategy.Deserialize(previous.Parameter?.ToString());
            return await NavigationOrchestratorAsync(previous.SourcePageType, parameter, NavigationMode.Back, () =>
            {
                if (CanGoBack)
                {
                    FrameFacadeInternal.GoBack(infoOverride);
                }
                return true;
            });
        }

        #endregion

        #region GoForward methods

        public bool CanGoForward => FrameFacadeInternal.CanGoForward;

        public void GoForward() => GoForwardAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task<bool> GoForwardAsync()
        {
            if (!CanGoForward)
            {
                return true;
            }
            var next = FrameFacadeInternal.ForwardStack.FirstOrDefault();
            var parameter = Settings.SerializationStrategy.Deserialize(next.Parameter?.ToString());
            return await NavigationOrchestratorAsync(next.SourcePageType, parameter, NavigationMode.Forward, () =>
            {
                if (CanGoForward)
                {
                    FrameFacadeInternal.GoForward();
                }
                return true;
            });
        }

        #endregion

        public void ClearHistory() => FrameFacadeInternal.BackStack.Clear();

        async Task INavigationServiceInternal.SuspendAsync()
        {
            DebugWrite($"Frame: {FrameFacadeInternal.FrameId}");

            // preserve date, start expiring the cache [optional] as of now
            await (await FrameFacadeInternal.GetFrameStateAsync()).SetCacheDateKeyAsync(DateTime.Now);

            var dispatcher = this.Window.Dispatcher;
            await dispatcher.DispatchAsync(async () =>
            {
                var vm = (FrameFacadeInternal.Content as Page)?.DataContext;
                var fromInfo = new NavigationInfo(CurrentPageType, CurrentPageParam, await FrameFacadeInternal.GetPageStateAsync(CurrentPageType));
                await Settings.ViewModelActionStrategy.NavigatedFromAsync((vm, NavigationMode.New, true), fromInfo, null, this);
            });
        }
    }
}

