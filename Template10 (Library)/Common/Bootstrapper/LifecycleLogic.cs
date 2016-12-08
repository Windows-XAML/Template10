using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Template10.Services.PopupService;
using Template10.Utils;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Template10.Services.ViewService;
using static Template10.Common.BootStrapper;
using System.Diagnostics;

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
