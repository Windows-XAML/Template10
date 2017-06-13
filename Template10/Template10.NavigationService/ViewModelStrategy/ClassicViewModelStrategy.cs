using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Portable.Navigation;
using Template10.Utils;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Classic = Template10.Services.NavigationService;

namespace Template10.Services.NavigationService
{

    public class ClassicViewModelStrategy : ViewModelStrategyBase
    {
        #region Debug

        private static void DebugWrite(string text = null, LoggingService.Severities severity = LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(ClassicViewModelStrategy)}.{caller}");

        #endregion

        public async override Task<bool> NavigatingToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            await Task.CompletedTask;
            return true;
        }

        public async override Task NavigatedToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            DebugWrite();

            if (to == null) throw new ArgumentNullException(nameof(to));
            if (operation.ViewModel is Classic.INavigatedAwareAsync vm && vm != null)
            {
                if (operation.NavigationMode == Windows.UI.Xaml.Navigation.NavigationMode.New) await to.PageState.ClearAsync();
                await vm?.OnNavigatedToAsync(to.Parameter, operation.NavigationMode, to.PageState);
                (nav.FrameFacade.Content as Page).UpdateBindings();
            }
        }

        public async override Task<bool> NavigatingFromAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            DebugWrite();

            if (operation.ViewModel is Classic.INavigatingAwareAsync vm && vm != null)
            {
                var deferral = new DeferralManager();
                var args = new Classic.NavigatingEventArgs(deferral)
                {
                    PageType = from.PageType,
                    Parameter = from.Parameter,
                    NavigationMode = operation.NavigationMode,
                    TargetPageType = to.PageType,
                    TargetPageParameter = to.Parameter,
                    Suspending = operation.Suspending,
                };
                await vm.OnNavigatingFromAsync(args);
                await deferral.WaitForDeferralsAsync();
                return args.Cancel;
            }
            else
            {
                return true;
            }
        }

        public async override Task NavigatedFromAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav)
        {
            DebugWrite();

            if (operation.ViewModel is Classic.INavigatedAwareAsync vm && vm != null)
            {
                await vm.OnNavigatedFromAsync(to.PageState, operation.Suspending);
            }
        }
    }
}