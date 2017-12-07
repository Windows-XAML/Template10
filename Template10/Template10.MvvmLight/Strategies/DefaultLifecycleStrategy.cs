using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.Messaging;
using Template10.Navigation;
using Template10.Common;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

namespace Template10.Strategies
{
    public class DefaultLifecycleStrategy : ILifecycleStrategy
    {
        IMessengerService _messengerService;
        public DefaultLifecycleStrategy(IMessengerService messengerService)
        {
            _messengerService = messengerService;
        }

        public Boolean PreviouslySuspended
        {
            get { return Windows.Storage.ApplicationData.Current.LocalSettings.Values[$"Template10-{nameof(PreviouslySuspended)}"] as Boolean? ?? false; }
            set { Windows.Storage.ApplicationData.Current.LocalSettings.Values[$"Template10-{nameof(PreviouslySuspended)}"] = value; }
        }

        public async Task SuspendAsync(ISuspendingEventArgs e)
        {
            Settings.LastSuspended = DateTime.Now;

            if (Settings.EnableLifecycleStrategy)
            {
                PreviouslySuspended = true;

                _messengerService.Send(new Messages.SuspendingMessage { EventArgs = e });

                // TODO: what to do with multiple views?

                foreach (var nav in NavigationService.Instances.Select(x => x as INavigationService2))
                {
                    await nav.SaveAsync(true);
                }
            }
        }

        public bool IsResuming(IStartArgsEx e)
        {
            if (Settings.AppAlwaysResumes
                && e?.StartKind == StartArgsEx.StartKinds.Launch
                && e?.StartCause == StartArgsEx.StartCauses.Primary)
            {
                return true;
            }

            if (e?.LaunchActivatedEventArgs?.PrelaunchActivated ?? false)
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

        public async Task ResumingAsync()
        {
            if (Settings.EnableLifecycleStrategy)
            {
                await Task.CompletedTask;
            }
        }

        public async Task<bool> ResumeAsync(IStartArgsEx e)
        {
            if (!Settings.EnableLifecycleStrategy)
            {
                return false;
            }

            if (!IsResuming(e))
            {
                return false;
            }

            PreviouslySuspended = false;

            if (e?.StartKind == StartArgsEx.StartKinds.Launch)
            {
                foreach (var nav in NavigationService.Instances.Select(x => x as INavigationService2))
                {
                    if (await nav.LoadAsync(true, Windows.UI.Xaml.Navigation.NavigationMode.New))
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
