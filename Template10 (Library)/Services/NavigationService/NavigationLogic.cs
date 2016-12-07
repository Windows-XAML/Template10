using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Template10.Utils;
using System.Diagnostics;

namespace Template10.Services.NavigationService
{

    public class NavigationLogic
    {
        internal NavigationLogic()
        {

        }

        public INavigationService NavigationService { get; set; }
        public FrameFacade FrameFacade { get; set; }

        private Windows.Foundation.Collections.IPropertySet State(Page page)
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
                throw new ArgumentNullException(nameof(viewmodel));
            }
            viewmodel.NavigationService = service;
            viewmodel.Dispatcher = service.GetDispatcherWrapper();
            viewmodel.SessionState = BootStrapper.Current.SessionState;
        }

        public async Task NavedFromAsync(Page page, INavigatedAwareAsync viewmodel, bool suspending)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }
            if (viewmodel == null)
            {
                throw new ArgumentNullException(nameof(viewmodel));
            }
            await viewmodel.OnNavigatedFromAsync(State(page), suspending);
        }

        public async Task NavedToAsync(object parameter, NavigationMode mode, Page page, INavigatedAwareAsync viewmodel)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }
            if (viewmodel == null)
            {
                throw new ArgumentNullException(nameof(viewmodel));
            }
            if (mode == NavigationMode.New)
            {
                State(page).Clear();
            }

            await viewmodel.OnNavigatedToAsync(parameter, mode, State(page));

            page.InitializeBindings();
            page.UpdateBindings();
        }

        public async Task<bool> NavingFromAsync(INavigatingAwareAsync viewmodel, Page page, object currentParameter, bool suspending, NavigationMode navigationMode, Type targetPageType, object targetParameter)
        {
            Services.NavigationService.NavigationService.DebugWrite();

            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }
            if (viewmodel == null)
            {
                throw new ArgumentNullException(nameof(viewmodel));
            }
            var deferral = new DeferralManager();
            var navigatingEventArgs = new NavigatingEventArgs(deferral)
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
            return !navigatingEventArgs.Cancel;
        }
    }

}