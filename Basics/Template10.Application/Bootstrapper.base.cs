using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Application
{

    public abstract partial class BootStrapper : IBootStrapper
    {
        private bool _hasInitializeAsync;

        private static SemaphoreSlim _startSemaphore = new SemaphoreSlim(1, 1);

        public virtual void Initialize(StartArgs args) { /* empty */ }

        public virtual Task InitializeAsync(StartArgs args) => Task.CompletedTask;

        public abstract Task StartAsync(StartArgs args, StartKinds activate);

        private async Task OrchestrateAsync(StartArgs startArgs, StartKinds activate)
        {
            await _startSemaphore.WaitAsync();
            try
            {
                if (!_hasInitializeAsync)
                {
                    _hasInitializeAsync = true;
                    Initialize(startArgs);
                    await InitializeAsync(startArgs);
                }
                await StartAsync(startArgs, activate);
                Window.Current.Activate();
            }
            finally
            {
                _startSemaphore.Release();
            }
        }
    }
}
