using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Template10.Helpers;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Template10.WindowService;

namespace Template10.App
{
    public class LifecycleLogic : ILifecycleLogic
    {
        public bool UseExtendExecution { get; set; } = true;

        internal async Task SuspendAsync()
        {
            if (UseExtendExecution)
            {
                using (var session = CreateSession())
                {
                    foreach (var nav in ViewService.AllNavigationServices)
                    {
                        await SuspendAsync(nav);
                    }
                }
            }
            else
            {
                foreach (var nav in ViewService.AllNavigationServices)
                {
                    await SuspendAsync(nav);
                }
            }
        }

        internal async Task RestoreAsync()
        {
            foreach (var nav in ViewService.AllNavigationServices)
            {
                await RestoreAsync(nav);
            }
        }

        private async Task SuspendAsync(Navigation.INavigationService nav)
        {
            this.DebugWriteMessage($"NavigationService {nav}");
            var vm = nav.ViewModel as Suspension.ISuspensionAware;
            await vm?.OnSuspendingAsync(nav.SuspensionState.Mark());
        }

        private async Task RestoreAsync(Navigation.INavigationService nav)
        {
            this.DebugWriteMessage($"NavigationService {nav}");
            var vm = nav.ViewModel as Suspension.ISuspensionAware;
            await vm?.OnResumingAsync(nav.SuspensionState);
        }

        private Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionSession CreateSession()
        {
            return new Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionSession
            {
                Description = GetType().ToString(),
                Reason = Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionReason.SavingData
            };
        }
    }

}