using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.BCL;
using Template10.Services.Lifetime;
using Template10.Services.Serialization;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.Navigation
{
    public class NavigationService : INavigationService,
        IInfrastructure<INavigationStateService>,
        IInfrastructure<ISuspensionService>,
        IInfrastructure<ISerializationService>,
        IInfrastructure<IViewModelService>
    {
        INavigationStateService IInfrastructure<INavigationStateService>.Instance { get; set; } = NavigationStateService.Instance;
        ISerializationService IInfrastructure<ISerializationService>.Instance { get; set; } = SerializationService.Instance;
        ISuspensionService IInfrastructure<ISuspensionService>.Instance { get; set; } = SuspensionService.Instance;
        IViewModelService IInfrastructure<IViewModelService>.Instance { get; set; } = ViewModelService.Instance;

        IFrameFacadeInternal _frame;
        public NavigationService(Frame frame, string id)
        {
            Id = id;
            _frame = new FrameFacade(frame);
        }

        public async Task SuspendAsync()
        {
            var navigationStateService = this.GetInfrastructure<INavigationStateService>();
            var navigationState = _frame.GetNavigationState();
            if (!await navigationStateService.SaveToCacheAsync(Id, navigationState))
            {
                return;
            }

            var vm = CurrentViewModel as ISuspensionAware;
            if (vm != null)
            {
                var suspensionService = this.GetInfrastructure<ISuspensionService>();
                var suspensionState = await suspensionService.GetStateAsync(Id, CurrentPage?.GetType(), _frame.BackStack.Count);
                await vm.OnSuspendingAsync(suspensionState);
            }
        }

        public async Task ResumeAsync()
        {
            var navigationStateService = this.GetInfrastructure<INavigationStateService>();
            var navigationState = await navigationStateService.LoadFromCacheAsync(Id);
            if (!string.IsNullOrEmpty(navigationState))
            {
                _frame.SetNavigationState(navigationState);
            }

            var vm = CurrentViewModel as ISuspensionAware;
            if (vm != null)
            {
                var suspensionService = this.GetInfrastructure<ISuspensionService>();
                var suspensionState = await suspensionService.GetStateAsync(Id, CurrentPage?.GetType(), _frame.BackStack.Count);
                await vm.OnResumingAsync(suspensionState);
            }
        }

        private async Task<bool> InternalNavigateAsync(Type page, string parameter, NavigationModes mode, Func<bool> navigate)
        {
            if ((page?.FullName == CurrentPage?.GetType().FullName) && (parameter?.Equals(CurrentParameter) ?? false))
            {
                return false;
            }
            var confirmNavigate = CurrentViewModel as IConfirmNavigation;
            if (!await confirmNavigate?.CanNavigateAsync())
            {
                return false;
            }
            var navigationAware = CurrentViewModel as INavigationAware;
            await navigationAware?.OnNavigatedFromAsync();
            if (navigate?.Invoke() ?? false)
            {
                var viewModelService = this.GetInfrastructure<IViewModelService>();
                navigationAware = viewModelService.ResolveForPage(CurrentPage) as INavigationAware;
                await navigationAware?.OnNavigatedToAsync(parameter, mode);
                CurrentNavigationMode = mode;
                CurrentParameter = parameter;
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Id { get; set; }

        public Page CurrentPage => _frame.Content as Page;

        public string CurrentParameter { get; set; }

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
            if (!CanGoBack)
            {
                return false;
            }
            return await InternalNavigateAsync(null, null, NavigationModes.Back, () => _frame.GoBack());
        }

        public async Task<bool> GoForwardAsync()
        {
            if (!CanGoForward)
            {
                return false;
            }
            return await InternalNavigateAsync(null, null, NavigationModes.Forward, () => _frame.GoForward());
        }

        public async Task<bool> NavigateAsync(Type page, string parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            return await InternalNavigateAsync(page, parameter, NavigationModes.New, () =>
            {
                return _frame.Navigate(page, parameter, infoOverride);
            });
        }

        public async Task<bool> NavigateAsync<T>(T key, string parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible
        {
            var keys = App.Settings.PageKeys<T>();
            if (!keys.ContainsKey(key))
            {
                throw new KeyNotFoundException($"{nameof(key)}: {key}");
            }
            return await NavigateAsync(keys[key], parameter, infoOverride).ConfigureAwait(false);
        }

        #endregion  
    }
}
