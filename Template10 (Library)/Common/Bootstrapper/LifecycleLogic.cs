using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using static Template10.Common.BootStrapper;

namespace Template10.Common
{
    public class BootstrapperLifecycleLogic
    {
        private const string CacheDateKey = "Setting-Cache-Date";

        public async Task<bool> AutoRestoreAsync(ILaunchActivatedEventArgs e, INavigationService nav)
        {
            var restored = false;
            var launchedEvent = e as ILaunchActivatedEventArgs;
            if (BootStrapper.DetermineStartCause(e) == AdditionalKinds.Primary || launchedEvent?.TileId == "")
            {
                restored = await nav.LoadAsync();
            }
            return restored;
        }

        public async Task AutoSuspendAllFramesAsync(BootStrapper bootstrapper, SuspendingEventArgs e)
        {
            if (!bootstrapper.AutoSuspendAllFrames)
            {
                return;
            }
            if (bootstrapper.AutoExtendExecutionSession)
            {
                using (var session = new Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionSession
                {
                    Description = GetType().ToString(),
                    Reason = Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionReason.SavingData
                })
                {
                    await SuspendAllFramesAsync();
                }
            }
            else
            {
                await SuspendAllFramesAsync();
            }
        }

        private async Task SuspendAllFramesAsync()
        {
            //allow only main view NavigationService as others won't be able to use Dispatcher and processing will stuck
            var services = WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices).Where(x => x.IsInMainView);
            foreach (INavigationService nav in services)
            {
                try
                {
                    // call view model suspend (OnNavigatedfrom)
                    // date the cache (which marks the date/time it was suspended)
                    nav.Suspension.GetFrameState().Write(CacheDateKey, DateTime.Now.ToString());
                    await (nav as INavigationService).GetDispatcherWrapper().DispatchAsync(async () => await nav.SuspendingAsync());
                }
                catch
                {
                    Debugger.Break();
                }
            }
        }
    }

}
