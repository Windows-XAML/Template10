using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Classic = Template10.Services.NavigationService;
using Portable = Prism.Navigation;
using Template10.Utils;
using System.Diagnostics;

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
                var vm = viewmodel as Classic.INavigatedAwareAsync;
                await vm?.OnNavigatedFromAsync(PageState(sourcePage), suspending);
            }
            else if (viewmodel is Portable.INavigatedAware)
            {
                var vm = viewmodel as Portable.INavigatedAware;
                var parameters = new Portable.NavigationParameters();
                vm?.OnNavigatedFrom(parameters);
            }
            else if (viewmodel is Portable.INavigatedAwareAsync)
            {
                var vm = viewmodel as Portable.INavigatedAwareAsync;
                var parameters = new Portable.NavigationParameters();
                await vm?.OnNavigatedFromAsync(parameters);
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
                var vm = viewmodel as Classic.INavigatedAwareAsync;
                await vm?.OnNavigatedToAsync(targetParameter, mode, PageState(targetPage));
            }
            else if (viewmodel is Portable.INavigatedAware)
            {
                var parameters = new Portable.NavigationParameters();
                parameters.Add("NavigationMode", mode.ToPrismNavigationMode());
                parameters.Add("SourceType", sourceType);
                parameters.Add("SourceParameter", sourceParameter);
                parameters.Add("TargetType", targetType);
                parameters.Add("TargetParameter", targetParameter);
                var vm = viewmodel as Portable.INavigatedAware;
                vm?.OnNavigatedTo(parameters);
            }
            else if (viewmodel is Portable.INavigatedAwareAsync)
            {
                var parameters = new Portable.NavigationParameters();
                parameters.Add("NavigationMode", mode.ToPrismNavigationMode());
                parameters.Add("SourceType", sourceType);
                parameters.Add("SourceParameter", sourceParameter);
                parameters.Add("TargetType", targetType);
                parameters.Add("TargetParameter", targetParameter);
                var vm = viewmodel as Portable.INavigatedAwareAsync;
                await vm?.OnNavigatedToAsync(parameters);
            }

            targetPage.InitializeBindings();
            targetPage.UpdateBindings();
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
                    await vm?.OnNavigatingFromAsync(navigatingEventArgs);
                    await deferral.WaitForDeferralsAsync();
                }
                catch
                {
                    Debugger.Break();
                }
                return navigatingEventArgs.Cancel;
            }
            else if (viewmodel is Portable.IConfirmNavigationAsync)
            {
                var parameters = new Portable.NavigationParameters();
                parameters.Add("NavigationMode", mode.ToPrismNavigationMode());
                parameters.Add("SourceType", sourceType);
                parameters.Add("SourceParameter", sourceParameter);
                parameters.Add("TargetType", targetType);
                parameters.Add("TargetParameter", targetParameter);
                parameters.Add("Suspending", suspending);
                var vm = viewmodel as Portable.IConfirmNavigationAsync;
                return !await vm?.CanNavigateAsync(parameters);
            }
            else
            {
                return true;
            }
        }
    }
}