using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

namespace Template10.Common
{
    public interface ILifecycleStrategy
    {
        bool RunPersistStrategy { get; set; }
        bool RunRestoreStrategy { get; set; }

        bool Resuming(OnStartEventArgs e);
        Task<bool> RestoreAsync(OnStartEventArgs e);
        Task PersistAsync(SuspendingEventArgs e);

        event EventHandler<HandledEventArgs<OnStartEventArgs>> Resuming;
        event EventHandler<HandledEventArgs<SuspendingEventArgs>> Suspending;
    }

    public class DefaultLifecycleStrategy : ILifecycleStrategy
    {
        public bool RunPersistStrategy { get; set; } = true;
        public bool RunRestoreStrategy { get; set; } = true;

        public event EventHandler<HandledEventArgs<OnStartEventArgs>> Resuming;
        public event EventHandler<HandledEventArgs<SuspendingEventArgs>> Suspending;

        public async Task PersistAsync(SuspendingEventArgs e)
        {
            var args = new HandledEventArgs<SuspendingEventArgs>(e);
            Suspending?.Invoke(this, args);
            if (!RunPersistStrategy | args.Handled)
            {
                return;
            }

            // TODO: what to do with multiple views?

            foreach (var nav in NavigationServiceHelper.Instances.Select(x => x as INavigationServiceInternal))
            {
                await nav.SaveAsync();
            }
        }

        public bool Resuming(OnStartEventArgs e)
        {
            switch (e.PreviousExecutionState)
            {
                // if the app was previous suspended (and not terminated)
                case ApplicationExecutionState.Suspended:
                // if the app was previous suspended (and terminate)
                case ApplicationExecutionState.Terminated:
                // 20170615 bug: UWP now reports suspended apps as NotRunning
                case ApplicationExecutionState.NotRunning:
                    return true;
                default:
                    return false;
            }
        }

        public async Task<bool> RestoreAsync(OnStartEventArgs e)
        {
            var args = new HandledEventArgs<OnStartEventArgs>(e);
            Resuming?.Invoke(this, args);
            if (!RunRestoreStrategy | args.Handled)
            {
                return false;
            }

            if (!Resuming(e)) {
                return false;
            }

            if (e.StartKind == StartKinds.Launch)
            {
                foreach (var nav in NavigationServiceHelper.Instances.Select(x => x as INavigationServiceInternal))
                {
                    await nav.LoadAsync();
                }
            }
            return true;
        }
    }
}
