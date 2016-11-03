using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.BCL;
using Template10.Utils;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.Navigation
{
    public class NavigationService : INavigationService,
        ILogicHost<IFrameLogic>,
        ILogicHost<ISuspensionLogic>,
        ILogicHost<IViewModelLogic>,
        ILogicHost<INavigatedLogic>,
        ILogicHost<INavigatingLogic>
    {

        #region extensible logic

        IFrameLogic ILogicHost<IFrameLogic>.Instance { get; set; } = Navigation.FrameLogic.Instance;
        ISuspensionLogic ILogicHost<ISuspensionLogic>.Instance { get; set; } = Navigation.SuspensionLogic.Instance;
        IViewModelLogic ILogicHost<IViewModelLogic>.Instance { get; set; } = Navigation.ViewModelLogic.Instance;
        INavigatingLogic ILogicHost<INavigatingLogic>.Instance { get; set; } = Navigation.NavigatingLogic.Instance;
        INavigatedLogic ILogicHost<INavigatedLogic>.Instance { get; set; } = Navigation.NavigatedLogic.Instance;

        IFrameLogic FrameLogic => this.GetProvider<IFrameLogic>();
        ISuspensionLogic SuspensionLogic => this.GetProvider<ISuspensionLogic>();
        IViewModelLogic ViewModelLogic => this.GetProvider<IViewModelLogic>();
        INavigatedLogic NavigatedLogic => this.GetProvider<INavigatedLogic>();
        INavigatingLogic NavigatingLogic => this.GetProvider<INavigatingLogic>();

        #endregion

        public event EventHandler Suspending;
        public event EventHandler Suspended;
        public event EventHandler Resuming;
        public event EventHandler Resumed;
        public event EventHandler<Type> Navigating;
        public event EventHandler<bool> Navigated;

        IFrameFacadeInternal _frame;
        public NavigationService(Frame frame, string id)
        {
            this.DebugWriteInfo($"id: {id}");

            Id = id;
            _frame = new FrameFacade(frame);
        }

        public string Id { get; set; }

        public Page CurrentPage => _frame.Content as Page;

        public string CurrentParameter => _frame.BackStack.LastOrDefault()?.Parameter?.ToString();

        public NavigationModes CurrentNavigationMode { get; set; }

        public object CurrentViewModel => CurrentPage?.DataContext;

        public bool CanGoBack => _frame.CanGoBack;

        public bool CanGoForward => _frame.CanGoForward;

        public IReadOnlyList<IStackEntry> BackStack => _frame.BackStack;

        public IReadOnlyList<IStackEntry> ForwardStack => _frame.ForwardStack;

        public void ClearBackStack() => _frame.ClearBackStack();

        public async Task SuspendAsync()
        {
            this.DebugWriteInfo();

            Suspending?.Invoke(this, EventArgs.Empty);

            // save the navigation state
            await FrameLogic.SaveNavigationState(Id, _frame);

            // call the view-model's OnSuspending
            await SuspensionLogic.CallSuspendAsync(CurrentViewModel as ISuspensionAware);

            Suspended?.Invoke(this, EventArgs.Empty);
        }

        public async Task ResumeAsync()
        {
            this.DebugWriteInfo();

            Resuming?.Invoke(this, EventArgs.Empty);

            // restore the navigation state
            await FrameLogic.LoadNavigationState(Id, _frame);

            // call the view-models OnResuming
            await SuspensionLogic.CallResumeAsync(CurrentViewModel as ISuspensionAware);

            Resumed?.Invoke(this, EventArgs.Empty);
        }

        public async Task<bool> GoBackAsync(NavigationTransitionInfo infoOverride = null)
        {
            this.DebugWriteInfo();

            if (!CanGoBack)
            {
                return false;
            }
            return await InternalNavigateAsync(null, null, NavigationModes.Back, () => _frame.GoBack());
        }

        public async Task<bool> GoForwardAsync()
        {
            this.DebugWriteInfo();

            if (!CanGoForward)
            {
                return false;
            }
            return await InternalNavigateAsync(null, null, NavigationModes.Forward, () => _frame.GoForward());
        }

        public async Task<bool> NavigateAsync(Type page, string parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            this.DebugWriteInfo();

            return await NavigateAsync(page, parameter.ToPropertySet(), infoOverride);
        }

        public async Task<bool> NavigateAsync(Type page, IPropertySet parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            this.DebugWriteInfo();

            return await InternalNavigateAsync(page, parameter, NavigationModes.New, () =>
            {
                return _frame.Navigate(page, parameter, infoOverride);
            });
        }

        public async Task<bool> NavigateAsync<T>(T key, string parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible
        {
            this.DebugWriteInfo();

            return await NavigateAsync(key, parameter.ToPropertySet(), infoOverride);
        }

        public async Task<bool> NavigateAsync<T>(T key, IPropertySet parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible
        {
            this.DebugWriteInfo();

            var keys = App.Settings.PageKeys<T>();
            if (!keys.ContainsKey(key))
            {
                this.DebugWriteError($"KeyNotFound {nameof(key)}: {key}");
                throw new KeyNotFoundException($"{nameof(key)}: {key}");
            }
            return await NavigateAsync(keys[key], parameter, infoOverride).ConfigureAwait(false);
        }

        public async Task OpenAsync(Type page)
        {
            this.DebugWriteInfo($"page: {page}");

            await Task.CompletedTask; // TODO
        }

        #region private methods

        private async Task<bool> InternalNavigateAsync(Type page, IPropertySet parameter, NavigationModes mode, Func<bool> navigate)
        {
            this.DebugWriteInfo($"page: {page} parameter: {parameter} mode: {mode}");

            // if the identical page+parameter, then don't go
            if ((page?.FullName == CurrentPage?.GetType().FullName) && (parameter?.Equals(CurrentParameter) ?? false))
            {
                return false;
            }

            // if confirmation required, then ask
            var confirmNavigate = CurrentViewModel as IConfirmNavigation;
            if (confirmNavigate != null && !await confirmNavigate.CanNavigateAsync())
            {
                return false;
            }

            var oldViewModel = CurrentViewModel;

            // call OnNavigatingFrom()
            await NavigatingLogic.CallNavigatingFromAsync(oldViewModel as INavigatingAware);

            // call onNavigatingTo() if developer-supported
            var newViewModel = ViewModelLogic.ResolveViewModel(page);
            await NavigatedLogic.CallNavigatedToAsync(newViewModel as INavigatedAware, parameter, mode);

            // navigate
            Navigating?.Invoke(this, page);
            var result = navigate.Invoke();
            Navigated?.Invoke(this, result);
            if (result)
            {
                CurrentNavigationMode = mode;
            }
            else
            {
                this.DebugWriteError($"Navigation failed.");
                return false;
            }

            // call OnNavigatedFrom()
            await NavigatedLogic.CallNavigatedFromAsync(oldViewModel as INavigationAware);

            // setup or accept view-model
            if (CurrentViewModel == null)
            {
                if (newViewModel == null)
                {
                    CurrentPage.DataContext = ViewModelLogic.ResolveViewModel(CurrentPage);
                }
                else
                {
                    CurrentPage.DataContext = newViewModel;
                }
            }

            // call OnNavigatedTo()
            newViewModel = newViewModel ?? ViewModelLogic.ResolveViewModel(page);
            await NavigatedLogic.CallNavigatedToAsync(newViewModel as INavigatedAware, parameter, mode);

            return true;
        }

        #endregion
    }
}
