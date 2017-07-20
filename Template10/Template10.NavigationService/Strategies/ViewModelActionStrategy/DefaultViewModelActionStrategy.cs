using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Portable.Navigation;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using T10v2 = Template10.Portable.Navigation;

namespace Template10.Strategies
{

    public class DefaultViewModelActionStrategy : IViewModelActionStrategy
    {
        #region Debug

        private static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(DefaultViewModelActionStrategy)}.{caller}");

        #endregion

        public IDictionary<string, object> SessionState { get; set; } = SessionStateHelper.Current;

        public async Task<bool> NavigatingToAsync((object ViewModel, NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            DebugWrite();

            if (operation.ViewModel is T10v2.INavigatingToAwareAsync vm && vm != null)
            {
                var parameters = new T10v2.NavigatingToParameters(operation.NavigationMode.ToPortableNavigationMode(), from, to, operation.Resuming, SessionState);
                return await vm.OnNavigatingToAsync(parameters);
            }
            else
            {
                return true;
            }
        }

        public async Task NavigatedToAsync((object ViewModel, NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            DebugWrite();

            if (operation.ViewModel is T10v2.INavigatedToAwareAsync vm && vm != null)
            {
                var parameters = new T10v2.NavigatedToParameters(operation.NavigationMode.ToPortableNavigationMode(), from, to, operation.Resuming, SessionState);
                await vm.OnNavigatedToAsync(parameters);
                (nav.FrameFacade.Content as Page).UpdateBindings();
            }
        }

        public async Task<bool> NavigatingFromAsync((object ViewModel, NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            DebugWrite();

            if (!operation.Suspending && operation.ViewModel is T10v2.IConfirmNavigationAsync confirm && confirm != null)
            {
                var canParameters = new T10v2.ConfirmNavigationParameters(from, to, SessionState);
                if (!await confirm.CanNavigateAsync(canParameters)) return false;
            }
            if (operation.ViewModel is T10v2.INavigatingFromAwareAsync vm && vm != null)
            {
                var parameters = new T10v2.NavigatingFromParameters(operation.Suspending, from, to, SessionState);
                await vm.OnNavigatingFromAsync(parameters);
            }
            return false;
        }

        public async Task NavigatedFromAsync((object ViewModel, NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            DebugWrite();

            if (operation.ViewModel is T10v2.INavigatedFromAwareAsync vm && vm != null)
            {
                var parameters = new T10v2.NavigatedFromParameters(operation.Suspending, from, to, SessionState);
                await vm.OnNavigatedFromAsync(parameters);
            }
        }
    }
}