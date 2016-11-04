using System.Threading.Tasks;
using Template10.Services.Lifetime;
using Template10.Services.Navigation;
using Windows.ApplicationModel;

namespace Template10.App
{
    public class SuspensionLogic: ISuspensionLogic
    {
        public static SuspensionLogic Instance { get; } = new SuspensionLogic();
        private SuspensionLogic()
        {
            // private constructor
        }

        public async Task SuspendAsync(ISuspendingDeferral deferral)
        {
            this.LogInfo($"Settings.AutoSuspend: {Settings.AutoSuspend} Settings.AutoExtendExecution: {Settings.AutoExtendExecution}");

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
            this.LogInfo($"Settings.AutoRestore: {Settings.AutoRestore}");

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