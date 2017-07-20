using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.Messenger;
using Template10.Services.NavigationService;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

namespace Template10.Strategies
{
    public class DefaultLifecycleStrategy : ILifecycleStrategy
    {
        public async Task SuspendAsync(SuspendingEventArgs e)
        {
            Template10.Services.Messenger.MessengerService.Instance.Send(new SuspendingMessage { EventArgs = e });

            if (!Settings.RunSuspendStrategy)
            {
                return;
            }

            // TODO: what to do with multiple views?

            foreach (var nav in NavigationServiceHelper.Instances.Select(x => x as INavigationServiceInternal))
            {
                await nav.SaveAsync(true);
            }
        }

        public bool IsResuming(Template10StartArgs e)
        {
            if (Settings.AppAlwaysResumes && e.StartKind == StartKinds.Launch && e.StartCause == StartCauses.Primary)
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

        public async Task<bool> ResumeAsync(Template10StartArgs e)
        {
            if (!Settings.RunRestoreStrategy)
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
                    if (await nav.LoadAsync(true))
                    {
                        // 
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
