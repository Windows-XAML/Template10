using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Template10.Common
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-DispatcherWrapper
    public class DispatcherWrapper : IDispatcherWrapper
    {
        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"DispatcherWrapper.{caller}");

        #endregion

        public static IDispatcherWrapper Current() => WindowWrapper.Current().Dispatcher;

        internal DispatcherWrapper(CoreDispatcher dispatcher)
        {
            DebugWrite(caller: "Constructor");
            this.dispatcher = dispatcher;
        }

        public bool HasThreadAccess() => dispatcher.HasThreadAccess;

        private readonly CoreDispatcher dispatcher;

        public async Task DispatchAsync(Action action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            await Task.Delay(delayms);
            if (dispatcher.HasThreadAccess && priority == CoreDispatcherPriority.Normal) { action(); }
            else
            {
                var tcs = new TaskCompletionSource<object>();
                await dispatcher.RunAsync(priority, () =>
                {
                    try { action(); tcs.TrySetResult(null); }
                    catch (Exception ex) { tcs.TrySetException(ex); }
                });
                await tcs.Task.ConfigureAwait(false);
            }
        }

        public async Task DispatchAsync(Func<Task> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            await Task.Delay(delayms);
            if (dispatcher.HasThreadAccess && priority == CoreDispatcherPriority.Normal) { await func?.Invoke(); }
            else
            {
                var tcs = new TaskCompletionSource<object>();
                await dispatcher.RunAsync(priority, async () =>
                {
                    try { await func(); tcs.TrySetResult(null); }
                    catch (Exception ex) { tcs.TrySetException(ex); }
                });
                await tcs.Task.ConfigureAwait(false);
            }
        }

        public async Task<T> DispatchAsync<T>(Func<T> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            await Task.Delay(delayms);
            if (dispatcher.HasThreadAccess && priority == CoreDispatcherPriority.Normal) { return func(); }
            else
            {
                var tcs = new TaskCompletionSource<T>();
                await dispatcher.RunAsync(priority, () =>
                {
                    try { tcs.TrySetResult(func()); }
                    catch (Exception ex) { tcs.TrySetException(ex); }
                });
                return await tcs.Task.ConfigureAwait(false);
            }
        }

        public async void Dispatch(Action action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            await Task.Delay(delayms);
            if (dispatcher.HasThreadAccess && priority == CoreDispatcherPriority.Normal) { action(); }
            else
            {
                dispatcher.RunAsync(priority, () => action()).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        public T Dispatch<T>(Func<T> action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            Task.Delay(delayms).Wait();
            if (dispatcher.HasThreadAccess && priority == CoreDispatcherPriority.Normal) { return action(); }
            else
            {
                T result = default(T);
                dispatcher.RunAsync(priority, () => result = action()).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                return result;
            }
        }

        public async Task DispatchIdleAsync(Action action, int delayms = 0)
        {
            await Task.Delay(delayms);

            var tcs = new TaskCompletionSource<object>();
            await dispatcher.RunIdleAsync(delegate
            {
                try { action(); tcs.TrySetResult(null); }
                catch (Exception ex) { tcs.TrySetException(ex); }
            });
            await tcs.Task.ConfigureAwait(false);
        }

        public async Task DispatchIdleAsync(Func<Task> func, int delayms = 0)
        {
            await Task.Delay(delayms);

            var tcs = new TaskCompletionSource<object>();
            await dispatcher.RunIdleAsync(async delegate
            {
                try { await func(); tcs.TrySetResult(null); }
                catch (Exception ex) { tcs.TrySetException(ex); }
            });
            await tcs.Task.ConfigureAwait(false);
        }

        public async Task<T> DispatchIdleAsync<T>(Func<T> func, int delayms = 0)
        {
            await Task.Delay(delayms);

            var tcs = new TaskCompletionSource<T>();
            await dispatcher.RunIdleAsync(delegate
            {
                try { tcs.TrySetResult(func()); }
                catch (Exception ex) { tcs.TrySetException(ex); }
            });
            return await tcs.Task.ConfigureAwait(false);
        }

        public async void DispatchIdle(Action action, int delayms = 0)
        {
            await Task.Delay(delayms);

            dispatcher.RunIdleAsync(delegate { action(); }).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public T DispatchIdle<T>(Func<T> action, int delayms = 0) where T : class
        {
            Task.Delay(delayms).Wait();

            T result = null;
            dispatcher.RunIdleAsync(delegate { result = action(); }).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            return result;
        }
    }
}
