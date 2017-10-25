using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Navigation;
using Template10.Mvvm;
using Template10.Extensions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using T10v2 = Template10.Navigation;
using Template10.Core;

namespace Template10.Strategies
{

    public class DefaultViewModelActionStrategy : IViewModelActionStrategy
    {
        public DefaultViewModelActionStrategy(ISessionState sessionState)
        {
            this.SessionState = sessionState;
        }

        public ISessionState SessionState { get; private set; }

        public async Task<bool> NavigatingToAsync((object ViewModel, NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            this.Log($"{operation.ViewModel}");

            if (operation.ViewModel is T10v2.INavigatingToAwareAsync vm && vm != null)
            {
                var parameters = new T10v2.NavigatingToParameters(operation.NavigationMode.ToPortableNavigationMode(), from, to, operation.Resuming, this.SessionState);
                return await vm.OnNavigatingToAsync(parameters);
            }
            else
            {
                return true;
            }
        }

        public async Task NavigatedToAsync((object ViewModel, NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            this.Log($"{operation.ViewModel}");

            if (operation.ViewModel is T10v2.INavigatedToAwareAsync vm && vm != null)
            {
                var parameters = new T10v2.NavigatedToParameters(operation.NavigationMode.ToPortableNavigationMode(), from, to, operation.Resuming, SessionState);
                await vm.OnNavigatedToAsync(parameters);
                (nav.FrameEx.Content as Page).UpdateBindings();
            }
        }

        public async Task<ContinueResult> NavigatingFromAsync((object ViewModel, NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            this.Log($"{operation.ViewModel}");

            if (!operation.Suspending && operation.ViewModel is T10v2.IConfirmNavigationAsync confirm && confirm != null)
            {
                var canParameters = new T10v2.ConfirmNavigationParameters(from, to, SessionState);
                var canNavigate = await confirm.CanNavigateAsync(canParameters);
                if (!canNavigate)
                {
                    return ContinueResult.Stop;
                }
            }
            if (operation.ViewModel is T10v2.INavigatingFromAwareAsync vm && vm != null)
            {
                var parameters = new T10v2.NavigatingFromParameters(operation.Suspending, from, to, SessionState);
                await vm.OnNavigatingFromAsync(parameters);
            }
            return ContinueResult.Continue;
        }

        public async Task NavigatedFromAsync((object ViewModel, NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            this.Log($"{operation.ViewModel}");

            if (operation.ViewModel is T10v2.INavigatedFromAwareAsync vm && vm != null)
            {
                var parameters = new T10v2.NavigatedFromParameters(operation.Suspending, from, to, SessionState);
                await vm.OnNavigatedFromAsync(parameters);
            }
        }
    }
}