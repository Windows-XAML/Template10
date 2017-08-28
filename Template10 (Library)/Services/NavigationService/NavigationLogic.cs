using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Utils;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Classic = Template10.Services.NavigationService;
using Portable = Template10.Mobile.Services.NavigationService;

namespace Template10.Services.NavigationService
{

    public class NavigationLogic
    {
        public INavigationService NavigationService { get; }
        public NavigationLogic(NavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        private Windows.Foundation.Collections.IPropertySet PageState(Page page)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }
            return NavigationService.Suspension.GetPageState(page.GetType()).Values;
        }

        public void SetupViewModel(INavigationService service, ITemplate10ViewModel viewmodel)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (viewmodel == null)
            {
                return;
            }
            viewmodel.NavigationService = service;
            viewmodel.Dispatcher = service.GetDispatcherWrapper();
            viewmodel.SessionState = BootStrapper.Current.SessionState;
        }

        public async Task NavedFromAsync(object viewmodel, Page page, bool suspending)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (page == null)
            {
                return;
            }
            else if (viewmodel == null)
            {
                return;
            }
            else if (viewmodel is Classic.INavigatingAwareAsync)
            {
                var vm = viewmodel as Classic.INavigatedAwareAsync;
                await vm?.OnNavigatedFromAsync(PageState(page), suspending);
            }
            else if (viewmodel is Portable.INavigatingAwareAsync)
            {
                var vm = viewmodel as Portable.INavigatedAwareAsync;
                await vm?.OnNavigatedFromAsync(PageState(page), suspending);
            }
        }

        public async Task NavedToAsync(object viewmodel, object parameter, NavigationMode mode, Page page)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            if (mode == NavigationMode.New)
            {
                PageState(page).Clear();
            }

            if (viewmodel == null)
            {
                return;
            }
            else if (viewmodel is Classic.INavigatedAwareAsync)
            {
                var vm = viewmodel as Classic.INavigatedAwareAsync;
                await vm?.OnNavigatedToAsync(parameter, mode, PageState(page));
            }
            else if (viewmodel is Portable.INavigatedAwareAsync)
            {
                var vm = viewmodel as Portable.INavigatedAwareAsync;
                await vm?.OnNavigatedToAsync(parameter, mode.ToTemplate10NavigationMode(), PageState(page));
            }

            page.InitializeBindings();
            page.UpdateBindings();
        }

        #region INavigatingAwareAsync

        public async Task<bool> NavingFromCancelsAsync(object viewmodel, Page page, object currentParameter, bool suspending, NavigationMode navigationMode, Type targetPageType, object targetParameter)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (page == null)
            {
                return false;
            }
            else if (viewmodel == null)
            {
                return false;
            }
            else if (viewmodel is Classic.INavigatingAwareAsync)
            {
                var vm = viewmodel as Classic.INavigatingAwareAsync;
                return await NavingFromAsync(vm, page, currentParameter, suspending, navigationMode, targetPageType, targetParameter);
            }
            else if (viewmodel is Portable.INavigatingAwareAsync)
            {
                var vm = viewmodel as Portable.INavigatingAwareAsync;
                return await NavingFromAsync(vm, page, currentParameter, suspending, navigationMode, targetPageType, targetParameter);
            }
            else
            {
                return true;
            }
        }

        private async Task<bool> NavingFromAsync(Classic.INavigatingAwareAsync viewmodel, Page page, object currentParameter, bool suspending, NavigationMode navigationMode, Type targetPageType, object targetParameter)
        {
            var deferral = new DeferralManager();
            var navigatingEventArgs = new Classic.NavigatingEventArgs(deferral)
            {
                Page = page,
                Parameter = currentParameter,
                Suspending = suspending,
                NavigationMode = navigationMode,
                TargetPageType = targetPageType,
                TargetPageParameter = targetParameter,
            };
            try
            {
                await viewmodel.OnNavigatingFromAsync(navigatingEventArgs);
                await deferral.WaitForDeferralsAsync();
            }
            catch
            {
                Debugger.Break();
            }
            return navigatingEventArgs.Cancel;
        }

        private async Task<bool> NavingFromAsync(Portable.INavigatingAwareAsync viewmodel, Page page, object currentParameter, bool suspending, NavigationMode navigationMode, Type targetPageType, object targetParameter)
        {
            var deferral = new Template10.Mobile.Common.DeferralManager();
            var navigatingEventArgs = new Portable.NavigatingEventArgs(deferral)
            {
                Page = page,
                Parameter = currentParameter,
                Suspending = suspending,
                NavigationMode = navigationMode.ToTemplate10NavigationMode(),
                TargetPageType = targetPageType,
                TargetPageParameter = targetParameter,
            };
            try
            {
                await viewmodel.OnNavigatingFromAsync(navigatingEventArgs);
                await deferral.WaitForDeferralsAsync();
            }
            catch
            {
                Debugger.Break();
            }
            return navigatingEventArgs.Cancel;
        }

        #endregion
    }
}