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

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public partial class NavigationService : INavigationService
    {
        public static INavigationService GetForFrame(Frame frame) =>
            WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices).FirstOrDefault(x => x.Frame.Equals(frame));

        private readonly IViewService viewService = new ViewService.ViewService();

        public FrameFacade FrameFacade { get; private set; }

        [Obsolete("Use FrameFacade. This may be made private in future versions.", false)]
        public Frame Frame => FrameFacade.Frame;

        public bool IsInMainView { get; }

        public object CurrentPageParam { get; internal set; }

        public Type CurrentPageType { get; internal set; }

        public object Content => Frame.Content;

        public IDispatcherWrapper Dispatcher => this.GetDispatcherWrapper();

        public string NavigationState { get { return Frame.GetNavigationState(); } set { Frame.SetNavigationState(value); } }

        public ISerializationService SerializationService { get; set; } = Services.SerializationService.SerializationService.Json;

        protected internal NavigationService(Frame frame)
        {
            IsInMainView = CoreApplication.MainView == CoreApplication.GetCurrentView();
            FrameFacade = Nav.FrameFacade = new FrameFacade(frame);
            FrameFacade.BackRequested += async (s, e) =>
            {
                if (FrameFacade.BackButtonHandling == BootStrapper.BackButton.Attach && !e.Handled)
                {
                    await GoBackAsync();
                }
            };
            FrameFacade.ForwardRequested += async (s, e) =>
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

            var oldPage = Frame.Content as Page;
            var oldViewModel = oldPage?.DataContext;

            // oldViewModel.OnNavigatingFromAsync
            var oldNavigatingAwareAsync = oldViewModel as INavigatingAwareAsync;
            if (oldNavigatingAwareAsync != null)
            {
                var canNavigate = await Nav.NavingFromAsync(oldNavigatingAwareAsync);
                if (!canNavigate)
                {
                    return false;
                }
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

            // oldViewModel.OnNavigatedFrom()
            var oldNavigatedAwareAsync = oldViewModel as INavigatedAwareAsync;
            if (oldNavigatedAwareAsync != null)
            {
                await Nav.NavedFromAsync(oldPage, oldNavigatedAwareAsync);
            }

            var newPage = Frame.Content as Page;
            var newViewModel = newPage?.DataContext;

            // newViewModel.ResolveForPage
            if (newViewModel == null)
            {
                newPage.DataContext = BootStrapper.Current.ResolveForPage(newPage, this);
            }

            // newTemplate10ViewModel.Properties
            var newTemplate10ViewModel = newViewModel as ITemplate10ViewModel;
            if (newTemplate10ViewModel != null)
            {
                Nav.SetupViewModel(this, newTemplate10ViewModel);
            }

            // newViewModel.OnNavigatedToAsync()
            var newNavigatedAwareAsync = newViewModel as INavigatedAwareAsync;
            if (newNavigatedAwareAsync != null)
            {
                await Nav.NavedToAsync(parameter, mode, newPage, newNavigatedAwareAsync);
            }

            // finally 
            return true;
        }

        private NavLogic Nav { get; } = new NavLogic();

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
                return Frame.Navigate(page, serializedParameter, infoOverride);
            });
        }

        public void Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            DebugWrite($"Page: {page}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            NavigateAsync(page, parameter, infoOverride).ConfigureAwait(false).GetAwaiter().GetResult();
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
            return await NavigateAsync(page, parameter, infoOverride).ConfigureAwait(false);
        }

        public void Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : struct, IConvertible
        {
            DebugWrite($"Key: {key}, Parameter: {parameter}, NavigationTransitionInfo: {infoOverride}");

            NavigateAsync(key, parameter, infoOverride).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion

        #region Save/Load Navigation methods

        public event EventHandler<CancelEventArgs<Type>> BeforeSavingNavigation;

        public async Task SaveAsync()
        {
            DebugWrite($"Frame: {FrameFacade.FrameId}");

            if (CurrentPageType == null)
                return;
            var args = new CancelEventArgs<Type>(CurrentPageType);
            BeforeSavingNavigation?.Invoke(this, args);
            if (args.Cancel)
                return;

            var state = FrameFacade.PageStateSettingsService(GetType().ToString());
            if (state == null)
            {
                throw new InvalidOperationException("State container is unexpectedly null");
            }

            state.Write<string>("CurrentPageType", CurrentPageType.AssemblyQualifiedName);
            state.Write<object>("CurrentPageParam", CurrentPageParam);
            state.Write<string>("NavigateState", NavigationState);

            await Task.CompletedTask;
        }

        [Obsolete("SaveNavigationAsync() is obsolete - please use SaveAsync() instead.", true)]
        public async Task SaveNavigationAsync()
        {
            await SaveAsync();
        }

        public event TypedEventHandler<Type> AfterRestoreSavedNavigation;

        public async Task<bool> LoadAsync()
        {
            DebugWrite($"Frame: {FrameFacade.FrameId}");

            try
            {
                var state = FrameFacade.PageStateSettingsService(GetType().ToString());
                if (state == null || !state.Exists("CurrentPageType"))
                {
                    return false;
                }

                CurrentPageType = state.Read<Type>("CurrentPageType");
                CurrentPageParam = state.Read<object>("CurrentPageParam");
                NavigationState = state.Read<string>("NavigateState");

                while (FrameFacade.Frame.Content == null)
                {
                    await Task.Delay(1);
                }

                var newPage = Frame.Content as Page;
                var newViewModel = newPage?.DataContext as INavigatedAwareAsync;
                if (newViewModel != null)
                {
                    await Nav.NavedToAsync(CurrentPageParam, NavigationMode.Refresh, newPage, newViewModel);
                }

                AfterRestoreSavedNavigation?.Invoke(this, CurrentPageType);
                return true;
            }
            catch { return false; }
        }

        [Obsolete("RestoreSavedNavigationAsync is obsolete - please use LoadAsync() instead.")]
        public async Task<bool> RestoreSavedNavigationAsync()
        {
            return await LoadAsync();
        }

        #endregion

        #region Refresh methods

        public void Refresh()
        {
            RefreshAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<bool> RefreshAsync()
        {
            var history = Frame.BackStack.Last();
            return await InternalNavigateAsync(history.SourcePageType, history.Parameter, NavigationMode.Refresh, () =>
            {
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
                Frame.SetNavigationState(Frame.GetNavigationState());
                return true;
            });
        }

        [Obsolete("Use Refresh()", true)]
        public void Refresh(object param) { }

        #endregion

        #region GoBack methods

        public void GoBack(NavigationTransitionInfo infoOverride = null)
        {
            GoBackAsync(infoOverride).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<bool> GoBackAsync(NavigationTransitionInfo infoOverride = null)
        {
            if (!FrameFacade.Frame.CanGoBack)
            {
                return false;
            }
            var previous = Frame.BackStack.LastOrDefault();
            return await InternalNavigateAsync(previous.SourcePageType, previous.Parameter, NavigationMode.Back, () =>
            {
                FrameFacade.Frame.GoBack(infoOverride);
                return true;
            });
        }

        public bool CanGoBack => Frame.CanGoBack;

        #endregion

        #region GoForward methods

        public void GoForward()
        {
            GoForwardAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<bool> GoForwardAsync()
        {
            if (!FrameFacade.Frame.CanGoForward)
            {
                return false;
            }
            var next = Frame.ForwardStack.FirstOrDefault();
            return await InternalNavigateAsync(next.SourcePageType, next.Parameter, NavigationMode.Forward, () =>
            {
                FrameFacade.Frame.GoForward();
                return true;
            });
        }

        public bool CanGoForward => Frame.CanGoForward;

        #endregion

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
                if (FrameFacade.Frame.BackStackDepth == 0)
                    FrameFacade.Frame.CacheSize = 1;
                else
                    FrameFacade.Frame.CacheSize = FrameFacade.Frame.BackStackDepth;
            }

            FrameFacade.Frame.CacheSize = currentSize;
        }

        public void ClearHistory() { FrameFacade.Frame.BackStack.Clear(); }

        public void Resuming() { /* nothing */ }

        public async Task SuspendingAsync()
        {
            DebugWrite($"Frame: {FrameFacade.FrameId}");

            await SaveAsync();

            var page = Frame.Content as Page;
            var viewmodel = page?.DataContext as INavigatedAwareAsync;
            if (viewmodel != null)
            {
                await Nav.NavedFromAsync(page, viewmodel);
            }
        }
    }

    internal class NavLogic
    {
        public FrameFacade FrameFacade { get; set; }

        private Windows.Foundation.Collections.IPropertySet State(Page page)
        {
            return FrameFacade.PageStateSettingsService(page.GetType()).Values;
        }

        public void SetupViewModel(INavigationService service, ITemplate10ViewModel viewmodel)
        {
            NavigationService.DebugWrite();

            if (viewmodel == null)
            {
                throw new ArgumentNullException(nameof(viewmodel));
            }
            viewmodel.NavigationService = service;
            viewmodel.Dispatcher = service.GetDispatcherWrapper();
            viewmodel.SessionState = BootStrapper.Current.SessionState;
        }

        public async Task NavedFromAsync(Page page, INavigatedAwareAsync viewmodel)
        {
            NavigationService.DebugWrite();

            if (viewmodel == null)
            {
                throw new ArgumentNullException(nameof(viewmodel));
            }
            await viewmodel.OnNavigatedFromAsync(State(page), false);
        }

        public async Task NavedToAsync(object parameter, NavigationMode mode, Page page, INavigatedAwareAsync viewmodel)
        {
            NavigationService.DebugWrite();

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

        public async Task<bool> NavingFromAsync(INavigatingAwareAsync viewmodel)
        {
            NavigationService.DebugWrite();

            if (viewmodel == null)
            {
                throw new ArgumentNullException(nameof(viewmodel));
            }
            var deferral = new DeferralManager();
            var navigatingEventArgs = new NavigatingEventArgs(deferral);

            // HERE'S THE ERROR; IT HANGS RIGHT HERE (any await)

            try
            {
                Debugger.Break();
                await viewmodel.OnNavigatingFromAsync(null);
                await deferral.WaitForDeferralsAsync();
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            return !navigatingEventArgs.Cancel;
        }
    }
}

