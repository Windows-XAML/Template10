using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.StateService;
using Template10.Utils;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Classic = Template10.Services.NavigationService;
using CrossPlat = Template10.Portable.Navigation;

namespace Template10.Services.NavigationService
{
    public class OldNavigationStrategy : INavigationStrategy
    {
        public INavigationService NavigationService { get; }
        public OldNavigationStrategy(INavigationService navigationService)
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

        public async Task SetupViewModelAsync(INavigationService service, INavigable viewmodel)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (viewmodel == null)
            {
                return;
            }
            viewmodel.NavigationService = service;
            viewmodel.Dispatcher = service.GetDispatcherWrapper();
            viewmodel.SessionState = await Services.StateService.SettingsStateContainer.GetStateAsync(StateService.StateTypes.Session);
        }

        public async Task NavedFromAsync(object viewmodel, NavigationMode mode, Page sourcePage, Type sourceType, object sourceParameter, Page targetPage, Type targetType, object targetParameter, bool suspending)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (sourcePage == null)
            {
                return;
            }
            else if (viewmodel == null)
            {
                return;
            }
            else if (viewmodel is Classic.INavigatedAwareAsync)
            {
                await CallClassicOnNavigatedFrom(viewmodel, sourcePage, suspending);
            }
            else if (viewmodel is CrossPlat.INavigatedFromAwareAsync)
            {
                await CallPortableOnNavigatedFrom(viewmodel, sourcePage, sourceParameter, suspending);
            }
        }

        public async Task NavedToAsync(object viewmodel, NavigationMode mode, Page sourcePage, Type sourceType, object sourceParameter, Page targetPage, Type targetType, object targetParameter)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (targetPage == null)
            {
                throw new ArgumentNullException(nameof(targetPage));
            }
            if (mode == NavigationMode.New)
            {
                PageState(targetPage).Clear();
            }
            if (viewmodel == null)
            {
                return;
            }
            else if (viewmodel is Classic.INavigatedAwareAsync)
            {
                await CallClassicOnNavigatedTo(viewmodel, mode, targetPage, targetParameter);
            }
            else if (viewmodel is CrossPlat.INavigatedToAwareAsync)
            {
                await CallPortableOnNavigatedTo(viewmodel, mode, sourceType, sourceParameter, targetPage, targetType, targetParameter);
            }

            UpdateBindings(targetPage);
        }

        public async Task<bool> NavingFromCancelsAsync(object viewmodel, NavigationMode mode, Page sourcePage, Type sourceType, object sourceParameter, Page targetPage, Type targetType, object targetParameter, bool suspending)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (sourcePage == null)
            {
                return false;
            }
            else if (viewmodel == null)
            {
                return false;
            }
            else if (viewmodel is Classic.INavigatingAwareAsync)
            {
                var cancel = await CallClassicOnNavigatingFrom(viewmodel, mode, sourcePage, sourceParameter, targetType, targetParameter, suspending);
                return cancel;
            }
            else if (viewmodel is Portable.Navigation.IConfirmNavigationAsync)
            {
                var canNavigate = await CallPortableCanNavigateAsync(viewmodel, mode, sourceType, sourceParameter, targetType, targetParameter, suspending);
                if (!canNavigate)
                {
                    return true;
                }
                await CallPortableNavigatingFromAsync(viewmodel, mode, sourceType, sourceParameter, targetType, targetParameter, suspending);
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void UpdateBindings(Page page)
        {
            page.InitializeBindings();
            page.UpdateBindings();
        }

        private async Task CallClassicOnNavigatedFrom(object viewmodel, Page sourcePage, bool suspending)
        {
            var vm = viewmodel as Classic.INavigatedAwareAsync;
            await vm?.OnNavigatedFromAsync(PageState(sourcePage), suspending);
        }

        private async Task CallClassicOnNavigatedTo(object viewmodel, NavigationMode mode, Page targetPage, object targetParameter)
        {
            var vm = viewmodel as Classic.INavigatedAwareAsync;
            await vm?.OnNavigatedToAsync(targetParameter, mode, PageState(targetPage));
        }

        private static async Task<bool> CallClassicOnNavigatingFrom(object viewmodel, NavigationMode mode, Page sourcePage, object sourceParameter, Type targetType, object targetParameter, bool suspending)
        {
            var deferral = new DeferralManager();
            var navigatingEventArgs = new Classic.NavigatingEventArgs(deferral)
            {
                Page = sourcePage,
                PageType = sourcePage?.GetType(),
                Parameter = sourceParameter,
                NavigationMode = mode,
                TargetPageType = targetType,
                TargetPageParameter = targetParameter,
                Suspending = suspending,
            };
            try
            {
                var vm = viewmodel as Classic.INavigatingAwareAsync;
                if (vm != null)
                {
                    await vm.OnNavigatingFromAsync(navigatingEventArgs);
                    await deferral.WaitForDeferralsAsync();
                }
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
            return navigatingEventArgs.Cancel;
        }

        private async Task CallPortableOnNavigatedFrom(object viewmodel, Page sourcePage, object sourceParameter, bool suspending)
        {
            var vm = viewmodel as CrossPlat.INavigatedFromAwareAsync;
            var parameters = new NavigatedFromParameters();
            parameters.Parameter = sourceParameter;
            parameters.PageState = PageState(sourcePage);
            parameters.Suspending = suspending;
            await vm?.OnNavigatedFromAsync(parameters);
        }

        private async Task CallPortableOnNavigatedTo(object viewmodel, NavigationMode mode, Type sourceType, object sourceParameter, Page targetPage, Type targetType, object targetParameter)
        {
            var parameters = new NavigatedToParameters();
            parameters.PageState = PageState(targetPage);
            parameters.NavigationMode = mode.ToPortableNavigationMode();
            parameters.SourceType = sourceType;
            parameters.SourceParameter = sourceParameter;
            parameters.TargetType = targetType;
            parameters.TargetParameter = targetParameter;
            var vm = viewmodel as CrossPlat.INavigatedToAwareAsync;
            await vm?.OnNavigatedToAsync(parameters);
        }

        private static async Task<bool> CallPortableCanNavigateAsync(object viewmodel, NavigationMode mode, Type sourceType, object sourceParameter, Type targetType, object targetParameter, bool suspending)
        {
            var parameters = new ConfirmNavigationParameters
            {
                NavigationMode = mode.ToPortableNavigationMode(),
                SourceType = sourceType,
                SourceParameter = sourceParameter,
                TargetType = targetType,
                TargetParameter = targetParameter,
                Suspending = suspending,
            };
            var vm = viewmodel as Portable.Navigation.IConfirmNavigationAsync;
            var canNavigate = await vm?.CanNavigateAsync(parameters);
            return canNavigate;
        }

        private static async Task CallPortableNavigatingFromAsync(object viewmodel, NavigationMode mode, Type sourceType, object sourceParameter, Type targetType, object targetParameter, bool suspending)
        {
            var parameters = new NavigatingFromParameters
            {
                NavigationMode = mode.ToPortableNavigationMode(),
                SourceType = sourceType,
                SourceParameter = sourceParameter,
                TargetType = targetType,
                TargetParameter = targetParameter,
                Suspending = suspending,
            };
            var vm = viewmodel as Portable.Navigation.INavigatingFromAwareAsync;
            await vm?.OnNavigatingFromAsync(parameters);
        }
    }
}