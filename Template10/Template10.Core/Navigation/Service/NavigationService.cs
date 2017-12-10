using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Template10.Common;
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
        public static INavigationService Create(BackButton backButton, Frame frame = null)
        {
            frame = frame ?? new Frame();

            var existing = frame.GetNavigationService();
            if (existing != null)
            {
                return existing;
            }

            var service = new NavigationService(frame);

            FinishCreatingAsyncVoid(backButton, frame, service);

            return service;
        }

        private static async void FinishCreatingAsyncVoid(BackButton backButton, Frame frame, NavigationService service)
        {
            await Task.CompletedTask;

            // TODO: add this feature back in if you can figure out the reason for an err
            // await ClearExpiredCacheAsync(service);

            service.BackButtonHandling = backButton;

            if (backButton == BackButton.Attach)
            {
                frame.RegisterPropertyChangedCallback(Frame.CanGoBackProperty, (s, args)
                    => GestureService.UpdateBackButton(service.CanGoBack));
                Central.Messenger.Subscribe<Messages.BackRequestedMessage>(frame, async m =>
                {
                    await service.GoBackAsync();
                });
            }

            if (!Instances.Any())
            {
                Default = service;
            }
            Instances.Register(service);

            Central.Messenger.Send(new Messages.NavigationServiceCreatedMessage
            {
                NavigationService = service,
                BackButtonHandling = backButton,
                IsDefault = Default == service,
                Dispatcher = service.GetDispatcher()
            });
        }

        #region simplifiers

        static IGestureService GestureService
            => Central.DependencyService.Resolve<IGestureService>();

        static ISerializationService SerializationService
            => Central.DependencyService.Resolve<ISerializationService>();

        static IViewModelActionStrategy ViewModelActionStrategy
            => Central.DependencyService.Resolve<IViewModelActionStrategy>();

        static IViewModelResolutionStrategy ViewModelResolutionStrategy
            => Central.DependencyService.Resolve<IViewModelResolutionStrategy>();

        #endregion

        public static INavigationService Default { get; set; }

        public static NavigationServiceRegistry Instances { get; } = new NavigationServiceRegistry();

        async static Task ClearExpiredCacheAsync(INavigationService service)
        {
            // this is always okay to check
            if (IsCacheExpired())
            {
                // clear state in every nav service in every view
                var facade = service.FrameEx as IFrameEx2;
                var state = await facade.GetFrameStateAsync();
                await state.ClearAsync();
            }
        }

        static bool IsCacheExpired()
        {
            var lastSuspended = Settings.LastSuspended;
            var cacheAge = DateTime.Now.Subtract(lastSuspended);
            return cacheAge >= Settings.CacheMaxDuration;
        }

        Lazy<IViewService> _viewService = new Lazy<IViewService>(() => new ViewService());
        private IViewService ViewService => _viewService.Value;

        public IWindowEx Window { get; private set; }

        public IFrameEx FrameEx { get; private set; }

        IFrameEx2 FrameEx2 => FrameEx as IFrameEx2;

        public BackButton BackButtonHandling { get; set; }

        public object CurrentPageParam { get; internal set; }

        public Type CurrentPageType { get; internal set; }

        internal NavigationService()
        {
            Window = WindowExManager.Current();
        }

        internal NavigationService(Frame frame) : this()
        {
            FrameEx = Navigation.FrameEx.Create(frame, this as INavigationService);
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
            if (Settings.NavigationBehavior == NavigationBehaviors.Key)
            {
                throw new InvalidOperationException("Navigation settings prevent navigation by type.");
            }
            NavigateAsync(page, parameter, infoOverride).RunSynchronously();
        }

        public async Task<bool> NavigateAsync(string key, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            this.Log($"Key: {key}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");
            if (Settings.NavigationBehavior == NavigationBehaviors.Type)
            {
                throw new InvalidOperationException("Navigation settings prevent navigation by key.");
            }
            if (Settings.PageKeyRegistry.TryGetValue(key, out var type))
            {
                return await NavigateAsync(type, parameter, infoOverride);
            }
            else
            {
                throw new KeyNotFoundException($"Navigation.Settings.PageKeyRegistry does not contain {key}.");
            }
        }

        public void Navigate(string key, object parameter = null, NavigationTransitionInfo infoOverride = null)
            => NavigateAsync(key, parameter, infoOverride).RunSynchronously();

        static SemaphoreSlim _navigationOrchestratorAsyncSemaphore = new SemaphoreSlim(1, 1);
        private async Task<bool> NavigationOrchestratorAsync(Type page, object parameter, NavigationMode mode, Func<bool> navigate)
        {
            try
            {
                await _navigationOrchestratorAsyncSemaphore.WaitAsync();

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
                var NavigatingFromAsyncResult = await ViewModelActionStrategy.NavigatingFromAsync(fromViewModel, mode, false, from, to, this);
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
                    await ViewModelActionStrategy.NavigatingToAsync(newViewModel, mode, false, from, to, this);
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
                    await ViewModelActionStrategy.NavigatingToAsync(newViewModel, mode, false, from, to, this);
                }

                // raise Navigated event
                Two.RaiseNavigated(new NavigatedEventArgs()
                {
                    Parameter = parameter,
                    NavigationMode = mode,
                    PageType = newPage?.GetType(),
                });

                // call fromViewModel.OnNavigatedFrom()
                await ViewModelActionStrategy.NavigatedFromAsync(fromViewModel, mode, false, from, to, this);

                // call toTemplate10ViewModel.Properties
                if (newViewModel is ITemplate10ViewModel vm)
                {
                    vm.NavigationService = this;
                }

                // call newViewModel.OnNavigatedToAsync()
                await ViewModelActionStrategy.NavigatedToAsync(newViewModel, mode, false, from, to, this);

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
                _navigationOrchestratorAsyncSemaphore.Release();
            }
        }

        #endregion

        #region Refresh methods

        public void Refresh()
            => RefreshAsync().RunSynchronously();

        public void Refresh(object param)
            => RefreshAsync(param).RunSynchronously();

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

        public bool CanGoBack
            => FrameEx2.CanGoBack;

        public void GoBack(NavigationTransitionInfo infoOverride = null)
            => GoBackAsync(infoOverride).RunSynchronously();

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

        public bool CanGoForward
            => FrameEx2.CanGoForward;

        public void GoForward()
            => GoForwardAsync().RunSynchronously();

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
        public event EventHandler<Common.HandledEventArgsEx> BackRequested;
        public event EventHandler<Common.HandledEventArgsEx> ForwardRequested;
        public event EventHandler<CancelEventArgsEx<Type>> BeforeSavingNavigation;
    }
}

