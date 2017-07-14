using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace Template10.Strategies
{
    public class DefaultLifecycleStrategyStrategy : ILifecycleStrategyStrategy
    {
        public event EventHandler<HandledEventArgs<StartupInfo>> Resuming;
        public event EventHandler<HandledEventArgs<SuspendingEventArgs>> Suspending;

        public async Task SuspendAsync(SuspendingEventArgs e)
        {
            var args = new HandledEventArgs<SuspendingEventArgs>(e);
            Suspending?.Invoke(this, args);
            if (!Settings.RunSuspendStrategy | args.Handled)
            {
                return;
            }

            // TODO: what to do with multiple views?

            foreach (var nav in NavigationServiceHelper.Instances.Select(x => x as INavigationServiceInternal))
            {
                await nav.SaveAsync();
            }
        }

        public bool IsResuming(StartupInfo e)
        {
            if (Settings.AlwaysResume)
            {
                return true;
            }

            if (e.ThisIsPrelaunch)
            {
                return false;
            }

            if (!e.ThisIsFirstStart)
            {
                return false;
            }

            switch (e.PreviousExecutionState)
            {
                // if the app was previous suspended (and not terminated)
                case ApplicationExecutionState.Suspended:
                // if the app was previous suspended (and terminate)
                case ApplicationExecutionState.Terminated:
                case ApplicationExecutionState.NotRunning:
                default:
                    return false;
            }
        }

        public async Task<bool> ResumeAsync(StartupInfo e)
        {
            var args = new HandledEventArgs<StartupInfo>(e);
            Resuming?.Invoke(this, args);
            if (!Settings.RunRestoreStrategy | args.Handled)
            {
                return false;
            }

            if (!IsResuming(e))
            {
                return false;
            }

            if (e.StartKind == StartKinds.Launch)
            {
                foreach (var nav in NavigationServiceHelper.Instances.Select(x => x as INavigationServiceInternal))
                {
                    if (!await nav.LoadAsync())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
