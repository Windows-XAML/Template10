using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Portable.Navigation;
using Template10.Utils;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CrossPlat = Template10.Portable.Navigation;

namespace Template10.Services.NavigationService
{

    public class PortableViewModelStrategy : ViewModelStrategyBase
    {
        #region Debug

        private static void DebugWrite(string text = null, LoggingService.Severities severity = LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(PortableViewModelStrategy)}.{caller}");

        #endregion

        public async override Task<bool> NavigatingToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            DebugWrite();

            if (operation.ViewModel is CrossPlat.INavigatingToAwareAsync vm && vm != null)
            {
                var parameters = new CrossPlat.NavigatingToParameters(operation.NavigationMode.ToPortableNavigationMode(), from, to, base.SessionState);
                return await vm.OnNavigatingToAsync(parameters);
            }
            else
            {
                return true;
            }
        }

        public async override Task NavigatedToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            DebugWrite();

            if (operation.ViewModel is CrossPlat.INavigatedToAwareAsync vm && vm != null)
            {
                var parameters = new CrossPlat.NavigatedToParameters(operation.NavigationMode.ToPortableNavigationMode(), from, to, base.SessionState);
                await vm.OnNavigatedToAsync(parameters);
                (nav.FrameFacade.Content as Page).UpdateBindings();
            }
        }

        public async override Task<bool> NavigatingFromAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            DebugWrite();

            if (!operation.Suspending && operation.ViewModel is CrossPlat.IConfirmNavigationAsync confirm && confirm != null)
            {
                var canParameters = new CrossPlat.ConfirmNavigationParameters(from, to, base.SessionState);
                if (!await confirm.CanNavigateAsync(canParameters)) return false;
            }
            if (operation.ViewModel is CrossPlat.INavigatingFromAwareAsync vm && vm != null)
            {
                var parameters = new CrossPlat.NavigatingFromParameters(operation.Suspending, from, to, base.SessionState);
                await vm.OnNavigatingFromAsync(parameters);
            }
            return true;
        }

        public async override Task NavigatedFromAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            DebugWrite();

            if (operation.ViewModel is CrossPlat.INavigatedFromAwareAsync vm && vm != null)
            {
                var parameters = new CrossPlat.NavigatedFromParameters(operation.Suspending, operation.NavigationMode.ToPortableNavigationMode(), from, to, base.SessionState);
                await vm.OnNavigatedFromAsync(parameters);
            }
        }
    }
}