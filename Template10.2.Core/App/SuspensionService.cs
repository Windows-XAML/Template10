using System.Threading.Tasks;
using Template10.Services.Lifetime;
using Template10.Services.Navigation;
using Windows.ApplicationModel;

namespace Template10.App
{
    public class SuspensionService: ISuspensionService
    {
        public static SuspensionService Instance { get; } = new SuspensionService();
        private SuspensionService()
        {
            // private constructor
        }

        public async Task SuspendAsync(ISuspendingDeferral deferral)
        {
            this.DebugWriteInfo($"Settings.AutoSuspend: {Settings.AutoSuspend} Settings.AutoExtendExecution: {Settings.AutoExtendExecution}");

            try
            {
                if (!Settings.AutoSuspend)
                {
                    return;
                }
                else if (Settings.AutoExtendExecution)
                {
                    using (var session = CreateSession())
                    {
                        foreach (var nav in ViewService.AllNavigationServices)
                        {
                            await nav.SuspendAsync();
                        }
                    }
                }
                else
                {
                    foreach (var nav in ViewService.AllNavigationServices)
                    {
                        await nav.SuspendAsync();
                    }
                }
            }
            finally
            {
                deferral?.Complete();
            }
        }

        public async Task RestoreAsync()
        {
            this.DebugWriteInfo($"Settings.AutoRestore: {Settings.AutoRestore}");

            if (!Settings.AutoRestore)
            {
                return;
            }

            foreach (var nav in ViewService.AllNavigationServices)
            {
                await nav.ResumeAsync();
            }
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