using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;
using Template10.Extensions;
using Template10.Services.Gesture;
using Template10.Services.Logging;
using Template10.Services.Serialization;
using Template10.Strategies;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public partial class NavigationService
    {
        /// <remarks>
        /// The shell back button should only be setup one time.
        /// </remarks>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async Task<INavigationService> CreateAsync(BackButton backButton, Frame frame = null)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
                frame.RegisterPropertyChangedCallback(Frame.CanGoBackProperty, (s, args) 
                    => BackButtonService.UpdateBackButton(service.CanGoBack));
                // frame.Navigated += (s, args) => BackButtonService.UpdateBackButton(service.CanGoBack);
                // BackButtonService.BackRequested += async (s, e) => e.Handled = await service.GoBackAsync();
                Central.Messenger.Subscribe<Messages.BackRequestedMessage>(frame, async m =>
                {
                    await service.GoBackAsync();
                });
            }

            if (!Instances.Any())
            {
                Default = service;
            }
            Instances.Add(service);

            Central.Messenger.Send(new Messages.NavigationServiceCreatedMessage
            {
                NavigationService = service,
                BackButtonHandling = backButton,
                IsDefault = Default == service,
                Dispatcher = service.GetDispatcher()
            });
        }

        static IBackButtonService2 BackButtonService => Services.Dependency.DependencyService.Default.Resolve<IBackButtonService>() as IBackButtonService2;
        static ISerializationService SerializationService => Services.Dependency.DependencyService.Default.Resolve<ISerializationService>();
        static IViewModelActionStrategy ViewModelActionStrategy => Services.Dependency.DependencyService.Default.Resolve<IViewModelActionStrategy>();
        static IViewModelResolutionStrategy ViewModelResolutionStrategy => Services.Dependency.DependencyService.Default.Resolve<IViewModelResolutionStrategy>();

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
            this.Log($"Page: {page}, Parameter: {parameter}, Title: {title}, Size: {size}");

            var frame = new Frame();
            var nav = new NavigationService(frame);
            nav.Navigate(page, parameter);

            return ViewService.OpenAsync(frame, title, size);
        }

        #region Navigate methods

        public async Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            this.Log($"Page: {page}, Parameter: {parameter ?? "null"}, NavigationTransitionInfo: {infoOverride}");

            return await NavigationOrchestratorAsync(page, parameter, NavigationMode.New, () =>
            {
                try
                {
                    return FrameEx2.Navigate(page, parameter, infoOverride);
                }
                catch (Exception)
                {
                    Debugger.Break();
                    throw;
                }
            });
        }

        public void Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            this.Log($"Type: {page}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");
            if (Settings.NavigationMethod == Settings.NavigationMethods.Key)
            {
                throw new InvalidOperationException("Navigation settings prevent navigation by type.");
            }
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
            this.Log($"Key: {key}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");
            if (Settings.NavigationMethod == Settings.NavigationMethods.Type)
            {
                throw new InvalidOperationException("Navigation settings prevent navigation by key.");
            }
            var keys = Settings.PageKeys<T>();
            if (!keys.TryGetValue(key, out var page))
            {
                throw new KeyNotFoundException(key.ToString());
            }
            return await NavigateAsync(page, parameter, infoOverride);
        }

        public void Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : struct, IConvertible => NavigateAsync(key, parameter, infoOverride).ConfigureAwait(true);

        static SemaphoreSlim navigationOrchestratorAsyncSemaphore = new SemaphoreSlim(1, 1);
        private async Task<bool> NavigationOrchestratorAsync(Type page, object parameter, NavigationMode mode, Func<bool> navigate)
        {
            try
            {
                await navigationOrchestratorAsyncSemaphore.WaitAsync();

                this.Log($"Page: {page}, Parameter: {parameter}, NavigationMode: {mode}");

                if (page == null)
                {
                    throw new ArgumentNullException(nameof(page));
                }

                if (navigate == null)
                {
                    throw new ArgumentNullException(nameof(navigate));
                }

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

                // call fromViewModel.OnNavigatingFromAsync()
                var NavigatingFromAsyncResult = await ViewModelActionStrategy.NavigatingFromAsync((fromViewModel, mode, false), from, to, this);
                if (NavigatingFromAsyncResult == ContinueResult.Stop)
                {
                    return false;
                }

                // raise Navigating event
                Two.RaiseNavigatingCancels(parameter, false, mode, to, out var RaiseNavigatingCancelsResult);
                if (RaiseNavigatingCancelsResult == ContinueResult.Stop)
                {
                    return false;
                }

                // try to resolve the view-model before navigation
                var newViewModel = await ViewModelResolutionStrategy.ResolveViewModelAsync(page);
                if (newViewModel != null)
                {
                    if (newViewModel is ITemplate10ViewModel t)
                    {
                        t.NavigationService = this;
                    }
                    await ViewModelActionStrategy.NavigatingToAsync((newViewModel, mode, false), from, to, this);
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
                Two.RaiseNavigated(new NavigatedEventArgs()
                {
                    Parameter = parameter,
                    NavigationMode = mode,
                    PageType = newPage?.GetType(),
                });

                // call fromViewModel.OnNavigatedFrom()
                await ViewModelActionStrategy.NavigatedFromAsync((fromViewModel, mode, false), from, to, this);

                // call toTemplate10ViewModel.Properties
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
                this.Log(ex.Message, Severities.Error);
                Debugger.Break();
                throw;
            }
            finally
            {
                navigationOrchestratorAsyncSemaphore.Release();
            }
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

        public async Task<bool> RefreshAsync(object param)
        {
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

        public event EventHandler<NavigatedEventArgs> Navigated;
        public event EventHandler<NavigatingEventArgs> Navigating;
        public event EventHandler<HandledEventArgs> BackRequested;
        public event EventHandler<HandledEventArgs> ForwardRequested;
        public event EventHandler<CancelEventArgs<Type>> BeforeSavingNavigation;
    }
}

