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
        public Boolean PreviouslySuspended
        {
            get { return Windows.Storage.ApplicationData.Current.LocalSettings.Values["Template10-PreviousExecutionState"] as Boolean? ?? false; }
            set { Windows.Storage.ApplicationData.Current.LocalSettings.Values["Template10-PreviousExecutionState"] = value; }
        }

        public async Task SuspendAsync(SuspendingEventArgs e)
        {
            PreviouslySuspended = true;

            Template10.Services.Messenger.MessengerService.Instance.Send(new SuspendingMessage { EventArgs = e });

            // TODO: what to do with multiple views?

            foreach (var nav in NavigationServiceHelper.Instances.Select(x => x as INavigationServiceInternal))
            {
                await nav.SaveAsync(true);
            }
        }

        public bool IsResuming(Template10StartArgs e)
        {
            if (Settings.AppAlwaysResumes 
                && e.StartKind == Template10.Template10StartArgs.StartKinds.Launch 
                && e.StartCause == Template10.Template10StartArgs.StartCauses.Primary)
            {
                return true;
            }

            if (PreviouslySuspended)
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
                    return true;
                // if the app was previous suspended (and terminate)
                case ApplicationExecutionState.Terminated:
                case ApplicationExecutionState.NotRunning:
                default:
                    return false;
            }
        }

        public async Task<bool> ResumeAsync(Template10StartArgs e)
        {
            if (!IsResuming(e))
            {
                return false;
            }

            PreviouslySuspended = false;

            if (e.StartKind == Template10.Template10StartArgs.StartKinds.Launch)
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
