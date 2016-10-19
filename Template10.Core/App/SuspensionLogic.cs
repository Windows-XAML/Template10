using System;
using System.Threading.Tasks;
using Template10.Services.View;
using Template10.Interfaces.Services.Navigation;

namespace Template10.App
{
    public class SuspensionLogic 
    {
        public bool UseExtendExecution { get; set; } = true;

        internal async Task SuspendAsync()
        {
            this.DebugWriteMessage($"UseExtendExecution: {UseExtendExecution}");
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
            this.DebugWriteMessage();
            foreach (var nav in ViewService.AllNavigationServices)
            {
                await RestoreAsync(nav);
            }
        }

        private async Task SuspendAsync(INavigationService nav)
        {
            this.DebugWriteMessage($"NavigationService {nav}");
            var vm = nav.ViewModel as ISuspensionAware;
            await vm?.OnSuspendingAsync(nav.SuspensionState.Mark());
        }

        private async Task RestoreAsync(INavigationService nav)
        {
            this.DebugWriteMessage($"NavigationService {nav}");
            var vm = nav.ViewModel as ISuspensionAware;
            await vm?.OnResumingAsync(nav.SuspensionState);
        }

        private Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionSession CreateSession()
        {
            return new Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionSession
            {
                Description = this.GetType().ToString(),
                Reason = Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionReason.SavingData
            };
        }
    }

}