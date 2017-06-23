using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace Template10.Strategies.SuspendResumeStrategy
{
    public class DefaultSuspendResumeStrategy : ISuspendResumeStrategy
    {
        public event EventHandler<HandledEventArgs<StartupInfo>> Resuming;
        public event EventHandler<HandledEventArgs<SuspendingEventArgs>> Suspending;

        public async Task SuspendAsync(SuspendingEventArgs e)
        {
            SuspendedWorkaround = DateTime.Now.ToString();

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

        string SuspendedWorkaround
        {
            get => ApplicationData.Current.LocalSettings.Values[nameof(SuspendedWorkaround)]?.ToString() ?? string.Empty;
            set => ApplicationData.Current.LocalSettings.Values[nameof(SuspendedWorkaround)] = value;
        }

        public bool IsResuming(StartupInfo e)
        {
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
                // 20170615 bug: UWP now reports suspended apps as NotRunning
                case ApplicationExecutionState.NotRunning when !string.IsNullOrEmpty(SuspendedWorkaround):
                    return true;
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

            SuspendedWorkaround = string.Empty;

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
