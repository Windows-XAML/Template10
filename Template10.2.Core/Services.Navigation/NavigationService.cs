using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.BCL;
using Template10.Utils;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.Navigation
{
    public class NavigationService : INavigationService,
        IServiceHost<INavigationStateService>,
        IServiceHost<ISuspensionService>,
        IServiceHost<IViewModelService>
    {
        INavigationStateService IServiceHost<INavigationStateService>.Instance { get; set; } = NavigationStateService.Instance;
        ISuspensionService IServiceHost<ISuspensionService>.Instance { get; set; } = SuspensionService.Instance;
        IViewModelService IServiceHost<IViewModelService>.Instance { get; set; } = ViewModelService.Instance;

        IFrameFacadeInternal _frame;
        public NavigationService(Frame frame, string id)
        {
            Id = id;
            _frame = new FrameFacade(frame);
        }

        public event EventHandler Suspending;
        public event EventHandler Suspended;

        public async Task SuspendAsync()
        {
            this.DebugWriteInfo();
            Suspending?.Invoke(this, EventArgs.Empty);

            // save the navigation state
            var navigationStateService = this.GetService<INavigationStateService>();
            await navigationStateService.SaveNavigationState(Id, _frame);

            // call the view-model's OnSuspending
            var suspensionService = this.GetService<ISuspensionService>();
            await suspensionService.CallOnSuspendingAsync(Id, CurrentPage, _frame.BackStack.Count);

            Suspended?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Resuming;
        public event EventHandler Resumed;

        public async Task ResumeAsync()
        {
            this.DebugWriteInfo();
            Resuming?.Invoke(this, EventArgs.Empty);

            // restore the navigation state
            var navigationStateService = this.GetService<INavigationStateService>();
            await navigationStateService.LoadNavigationState(Id, _frame);

            // call the view-models OnResuming
            var suspensionService = this.GetService<ISuspensionService>();
            await suspensionService.CallOnResumingAsync(Id, CurrentPage, _frame.BackStack.Count);

            Resumed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<Type> Navigating;
        public event EventHandler<bool> Navigated;

        private async Task<bool> InternalNavigateAsync(Type page, string parameter, NavigationModes mode, Func<bool> navigate)
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

            var viewModelService = this.GetService<IViewModelService>();
            var oldViewModel = CurrentViewModel;

            // call OnNavigatingFrom()
            await viewModelService.CallNavigatingFromAsync(oldViewModel as INavigatingAware);

            // call onNavigatingTo()
            var newViewModel = viewModelService.ResolveViewModel(page);
            await viewModelService.CallNavigatingAsync(newViewModel as INavigatingAware, parameter, mode);

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
            await viewModelService.CallNavigatedFromAsync(oldViewModel as INavigationAware);

            if (CurrentViewModel == null)
            {
                if (newViewModel == null)
                {
                    CurrentPage.DataContext = viewModelService.ResolveViewModel(CurrentPage);
                }
                else
                {
                    CurrentPage.DataContext = newViewModel;
                }
            }

            // call OnNavigatedTo()
            newViewModel = newViewModel ?? viewModelService.ResolveViewModel(page);
            await viewModelService.CallNavigatingAsync(newViewModel as INavigatingAware, parameter, mode);


            // if navTo supported, then call it
            navigationAware = viewModelService.ResolveViewModel(CurrentPage) as INavigationAware;
            if (navigationAware != null)
            {
                await navigationAware?.OnNavigatedToAsync(parameter, mode);
                CurrentPage.UpdateBindings();
            }
            return true;
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

        #region Navigation

        public async Task<bool> GoBackAsync(NavigationTransitionInfo infoOverride = null)
        {
            this.DebugWriteInfo();

            if (!CanGoBack)
            {
                return false;
            }
            var result = await InternalNavigateAsync(null, null, NavigationModes.Back, () => _frame.GoBack());
            return result;
        }

        public async Task<bool> GoForwardAsync()
        {
            this.DebugWriteInfo();

            if (!CanGoForward)
            {
                return false;
            }
            var result = await InternalNavigateAsync(null, null, NavigationModes.Forward, () => _frame.GoForward());
            return result;
        }

        public async Task<bool> NavigateAsync(Type page, string parameter = null, NavigationTransitionInfo infoOverride = null)
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

        #endregion  
    }
}
