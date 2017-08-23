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
using Template10.Strategies;
using Template10.Services.Gesture;
using Template10.Services.Logging;
using Template10.Services.Serialization;
using Template10.Extensions;
using Template10.Core;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public partial class NavigationService : Loggable, INavigationService2
    {
        /// <summary>
        /// Creates a new NavigationService from the gived Frame to the
        /// WindowWrapper collection. In addition, it optionally will setup the
        /// shell back button to react to the nav of the Frame.
        /// A developer should call this when creating a new/secondary frame.
        /// </summary>
        /// <remarks>
        /// The shell back button should only be setup one time.
        /// </remarks>
        public static async Task<INavigationService> CreateAsync(BackButton backButton, Frame frame = null)
        {
            frame = frame ?? new Frame();

            var existing = frame.GetNavigationService();
            if (existing != null)
            {
                return existing;
            }

            var service = new NavigationService(frame);

            FinishCreatingAsync(backButton, frame, service);

            return service;
        }

        private static async void FinishCreatingAsync(BackButton backButton, Frame frame, NavigationService service)
        {
            await Task.CompletedTask;

            // TODO: add this feature back in if you can figure out the reason for an err
            // await ClearExpiredCacheAsync(service);

            service.BackButtonHandling = backButton;

            frame.RequestedTheme = Settings.DefaultTheme;

            if (backButton == BackButton.Attach)
            {
                // frame.RegisterPropertyChangedCallback(Frame.BackStackDepthProperty, (s, args) => BackButtonService.GetInstance().UpdateBackButton(service.CanGoBack));
                frame.Navigated += (s, args) => BackButtonService.UpdateBackButton(service.CanGoBack);
                BackButtonService.BackRequested += async (s, e) => e.Handled = await service.GoBackAsync();
            }

            if (!Instances.Any())
            {
                Default = service;
            }
            Instances.Add(service);

            Central.MessengerService.Send(new Messages.NavigationServiceCreatedMessage
            {
                NavigationService = service,
                BackButtonHandling = backButton,
                IsDefault = Default == service,
                Dispatcher = service.GetDispatcher()
            });
        }

        static IBackButtonService BackButtonService => Services.Container.ContainerService.Default.Resolve<IBackButtonService>();
        static ISerializationService SerializationService => Services.Container.ContainerService.Default.Resolve<ISerializationService>();
        static IViewModelActionStrategy ViewModelActionStrategy => Services.Container.ContainerService.Default.Resolve<IViewModelActionStrategy>();
        static IViewModelResolutionStrategy ViewModelResolutionStrategy => Services.Container.ContainerService.Default.Resolve<IViewModelResolutionStrategy>();

        private static object _PageKeys;
        public static Dictionary<T, Type> PageKeys<T>() where T : struct, IConvertible
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

        public static INavigationService Default { get; set; }
        public static NavigationServiceList Instances { get; } = new NavigationServiceList();


        async static Task ClearExpiredCacheAsync(INavigationService service)
        {
            // this is always okay to check, default or not, 
            // expire any state (based on expiry delta from today)

            // default the cache age to very fresh if not known
            var lastSuspended = Settings.LastSuspended;
            var cacheAge = DateTime.Now.Subtract(lastSuspended);

            // clear state in every nav service in every view
            if (cacheAge >= Settings.CacheMaxDuration)
            {
                var facade = service.FrameEx as IFrameEx2;
                var state = await facade.GetFrameStateAsync();
                await state.ClearAsync();
            }
        }

        Lazy<IViewService> _ViewService;
        private IViewService ViewService => _ViewService.Value;

        public IWindowEx Window { get; private set; }

        public IFrameEx FrameEx { get; private set; }
        IFrameEx2 FrameEx2 => FrameEx as IFrameEx2;

        public BackButton BackButtonHandling { get; set; }

        public object CurrentPageParam { get; internal set; }

        public Type CurrentPageType { get; internal set; }

        internal NavigationService()
        {
            Window = WindowEx.Current();
        }

        internal NavigationService(Frame frame) : this()
        {
            FrameEx = Navigation.FrameEx.Create(frame, this as INavigationService);
            _ViewService = new Lazy<IViewService>(() => new ViewService());
        }

        public Task<IViewLifetimeControl> OpenAsync(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf)
        {
            LogThis($"Page: {page}, Parameter: {parameter}, Title: {title}, Size: {size}");

            var frame = new Frame();
            var nav = new NavigationService(frame);
            nav.Navigate(page, parameter);

            return ViewService.OpenAsync(frame, title, size);
        }

        #region Navigate methods

        public async Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            LogThis($"Page: {page}, Parameter: {parameter ?? "null"}, NavigationTransitionInfo: {infoOverride}");

            return await NavigationOrchestratorAsync(page, parameter, NavigationMode.New, () =>
            {
                if (Settings.SerializeParameters)
                {
                    var serializedParameter = parameter.TrySerializeEx(out var result) ? result : throw new Exception("Parameter cannot be serialized.");
                    return FrameEx2.Navigate(page, serializedParameter, infoOverride);
                }
                else
                {
                    return FrameEx2.Navigate(page, parameter, infoOverride);
                }
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
            LogThis($"Key: {key}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            var keys = PageKeys<T>();
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
                LogThis($"Page: {page}, Parameter: {parameter}, NavigationMode: {mode}");

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
                var fromPage = FrameEx.Content as Page;
                var fromParameter = CurrentPageParam;
                var fromViewModel = fromPage?.DataContext;
                var fromPageName = fromPage?.GetType().ToString();
                var fromPageState = await FrameEx2.GetPageStateAsync(fromPage?.GetType());
                var from = new NavigationInfo(fromPage?.GetType(), fromParameter, fromPageState);

                var toPageName = page?.ToString();
                var toPageState = await FrameEx2.GetPageStateAsync(toPageName);
                var to = new NavigationInfo(page, parameter, toPageState);

                // call oldViewModel.OnNavigatingFromAsync()
                var result = await ViewModelActionStrategy.NavigatingFromAsync((fromViewModel, mode, false), from, to, this);
                if (result == ContinueResult.Stop)
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
                        newViewModel = await ViewModelResolutionStrategy.ResolveViewModelAsync(page);
                        if (newViewModel != null)
                        {
                            await ViewModelActionStrategy.NavigatingToAsync((newViewModel, mode, false), from, to, this);
                        }
                        break;
                }

                // navigate
                var newPage = default(Page);
                if (navigate.Invoke())
                {
                    if ((newPage = FrameEx.Content as Page) == null)
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
                    await ViewModelActionStrategy.NavigatingToAsync((newViewModel, mode, false), from, to, this);
                }

                // raise Navigated event
                RaiseNavigated(new NavigatedEventArgs()
                {
                    Parameter = parameter,
                    NavigationMode = mode,
                    PageType = newPage?.GetType(),
                });

                // call oldViewModel.OnNavigatedFrom()
                await ViewModelActionStrategy.NavigatedFromAsync((fromViewModel, mode, false), from, to, this);

                // call newTemplate10ViewModel.Properties
                if (newViewModel is ITemplate10ViewModel vm)
                {
                    vm.NavigationService = this;
                }

                // call newViewModel.OnNavigatedToAsync()
                await ViewModelActionStrategy.NavigatedToAsync((newViewModel, mode, false), from, to, this);

                // finally, all-good 
                return true;
            }
            catch (Exception ex)
            {
                LogThis(ex.Message);
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
        void INavigationService2.RaiseBackRequested(HandledEventArgs args)
        {
            BackRequested?.Invoke(this, args);
        }

        public event EventHandler<HandledEventArgs> ForwardRequested;
        void INavigationService2.RaiseForwardRequested(HandledEventArgs args)
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
        event TypedEventHandler<Type> INavigationService2.AfterRestoreSavedNavigation
        {
            add => afterRestoreSavedNavigation += value;
            remove => afterRestoreSavedNavigation -= value;
        }

        #endregion

        #region Save/Load Navigation methods

        async Task INavigationService2.SaveAsync(bool navigateFrom)
        {
            // save navigation state into settings

            LogThis($"Frame: {FrameEx.FrameId}");

            if (CurrentPageType == null) return;
            if (RaiseBeforeSavingNavigation()) return;

            if (navigateFrom)
            {
                var vm = (FrameEx.Content as Page)?.DataContext;
                var fromInfo = new NavigationInfo(CurrentPageType, CurrentPageParam, await FrameEx2.GetPageStateAsync(CurrentPageType));
                await ViewModelActionStrategy.NavigatingFromAsync((vm, NavigationMode.New, true), fromInfo, null, this);
                await ViewModelActionStrategy.NavigatedFromAsync((vm, NavigationMode.New, true), fromInfo, null, this);
            }

            var frameState = await FrameEx2.GetFrameStateAsync();
            var navigationState = FrameEx2.GetNavigationState();
            await frameState.SetNavigationStateAsync(navigationState);

            var writtenState = await frameState.TryGetNavigationStateAsync();
            LogThis($"navigationState:{navigationState} writtenState:{writtenState}");
            Debug.Assert(navigationState.Equals(writtenState.Value), "Checking frame nav state save");

            await Task.CompletedTask;
        }

        async Task<bool> INavigationService2.LoadAsync(bool navigateTo, NavigationMode mode)
        {
            LogThis($"Frame: {FrameEx.FrameId}");

            try
            {
                // load navigation state from settings
                var frameState = await FrameEx2.GetFrameStateAsync();
                {
                    var state = await frameState.TryGetNavigationStateAsync();
                    LogThis($"After TryGetNavigationStateAsync; state: {state.Value ?? "Null"}");
                    if (state.Success && !string.IsNullOrEmpty(state.Value?.ToString()))
                    {
                        FrameEx2.SetNavigationState(state.Value.ToString());
                        await frameState.SetNavigationStateAsync(string.Empty);
                    }
                    else
                    {
                        return false;
                    }
                }

                // is there a page there?
                var newPage = FrameEx.Content as Page;
                if (newPage == null)
                {
                    return false;
                }
                else
                {
                    CurrentPageType = newPage.GetType();
                }


                // continue?

                if (!navigateTo)
                {
                    return true;
                }

                // resolve?

                var viewModel = await ViewModelResolutionStrategy.ResolveViewModelAsync(newPage);
                if (viewModel == null)
                {
                    viewModel = newPage?.DataContext;
                }

                // known?

                if (viewModel is ITemplate10ViewModel vm && vm != null)
                {
                    vm.NavigationService = this;
                }

                // setup?

                if (viewModel != null)
                {
                    CurrentPageParam = FrameEx.BackStack.Last().Parameter;
                    if (CurrentPageParam != null && Settings.SerializeParameters)
                    {
                        CurrentPageParam = CurrentPageParam.ToString().DeserializeEx();
                    }
                    var toState = await FrameEx2.GetPageStateAsync(CurrentPageType);
                    var toInfo = new NavigationInfo(CurrentPageType, CurrentPageParam, toState);
                    await ViewModelActionStrategy.NavigatingToAsync((viewModel, mode, true), null, toInfo, this);
                    await ViewModelActionStrategy.NavigatedToAsync((viewModel, mode, true), null, toInfo, this);
                }

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
            await Two.SaveAsync(true);
            return await NavigationOrchestratorAsync(CurrentPageType, CurrentPageParam, NavigationMode.Refresh, () =>
            {
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
                return Two.LoadAsync(true, NavigationMode.Refresh).Result;
            });
        }

        INavigationService2 Two => this as INavigationService2;

        public async Task<bool> RefreshAsync(object param)
        {
            var history = FrameEx.BackStack.Last();
            return await NavigationOrchestratorAsync(CurrentPageType, param, NavigationMode.Refresh, () =>
            {
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
                FrameEx2.SetNavigationState(FrameEx2.GetNavigationState());
                return true;
            });
        }

        #endregion

        #region GoBack methods

        public bool CanGoBack => FrameEx2.CanGoBack;

        public void GoBack(NavigationTransitionInfo infoOverride = null) => GoBackAsync(infoOverride).ConfigureAwait(true);

        public async Task<bool> GoBackAsync(NavigationTransitionInfo infoOverride = null)
        {
            if (!CanGoBack)
            {
                return true;
            }
            var previous = FrameEx.BackStack.LastOrDefault();
            // there is no parameter when going forward and back
            return await NavigationOrchestratorAsync(previous.SourcePageType, null, NavigationMode.Back, () =>
            {
                if (CanGoBack)
                {
                    FrameEx2.GoBack(infoOverride);
                }
                return true;
            });
        }

        #endregion

        #region GoForward methods

        public bool CanGoForward => FrameEx2.CanGoForward;

        public void GoForward() => GoForwardAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task<bool> GoForwardAsync()
        {
            if (!CanGoForward)
            {
                return true;
            }
            var next = FrameEx.ForwardStack.FirstOrDefault();
            // there is no parameter when going forward and back
            return await NavigationOrchestratorAsync(next.SourcePageType, null, NavigationMode.Forward, () =>
            {
                if (CanGoForward)
                {
                    FrameEx2.GoForward();
                }
                return true;
            });
        }

        #endregion

        public void ClearHistory() => FrameEx.BackStack.Clear();

        async Task INavigationService2.SuspendAsync()
        {
            LogThis($"Frame: {FrameEx.FrameId}");

            var dispatcher = this.Window.Dispatcher;
            await dispatcher.DispatchAsync(async () =>
            {
                var vm = (FrameEx.Content as Page)?.DataContext;
                var fromInfo = new NavigationInfo(CurrentPageType, CurrentPageParam, await FrameEx2.GetPageStateAsync(CurrentPageType));
                await ViewModelActionStrategy.NavigatedFromAsync((vm, NavigationMode.New, true), fromInfo, null, this);
            });
        }
    }
}

