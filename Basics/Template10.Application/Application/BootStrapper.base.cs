using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
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

        public virtual void Start(StartArgs args, StartKinds activate) {  /* empty */ }

        public virtual Task StartAsync(StartArgs args, StartKinds activate) => Task.CompletedTask;

        private async Task InternalStartAsync(StartArgs startArgs, StartKinds activate)
        {
            Debug.WriteLine($"{nameof(BootStrapper)}.{nameof(InternalStartAsync)}({startArgs.Arguments}, {activate})");
            await _startSemaphore.WaitAsync();

            try
            {
                if (!_hasInitializeAsync)
                {
                    _hasInitializeAsync = true;

                    Debug.WriteLine($"{nameof(BootStrapper)}.{nameof(InternalStartAsync)}.{nameof(Initialize)}({startArgs.Arguments})");
                    Initialize(startArgs);

                    Debug.WriteLine($"{nameof(BootStrapper)}.{nameof(InternalStartAsync)}.{nameof(InitializeAsync)}({startArgs.Arguments})");
                    await InitializeAsync(startArgs);
                }

                Window.Current.Activate();

                Debug.WriteLine($"{nameof(BootStrapper)}.{nameof(InternalStartAsync)}.{nameof(Start)}({startArgs.Arguments}, {activate})");
                Start(startArgs, activate);

                Debug.WriteLine($"{nameof(BootStrapper)}.{nameof(InternalStartAsync)}.{nameof(StartAsync)}({startArgs.Arguments}, {activate})");
                await StartAsync(startArgs, activate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{nameof(BootStrapper)}.{nameof(InternalStartAsync)} exception: {ex}/{ex.Message})");
                Debugger.Break();
            }
            finally
            {
                _startSemaphore.Release();
            }
        }
    }
}
