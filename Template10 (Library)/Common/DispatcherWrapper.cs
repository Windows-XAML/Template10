using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Template10.Common
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-DispatcherWrapper
    public class DispatcherWrapper : IDispatcherWrapper
    {
        internal DispatcherWrapper(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public bool HasThreadAccess() => dispatcher.HasThreadAccess;

        private CoreDispatcher dispatcher;

        public async Task DispatchAsync(Action action, int delayms = 0)
        {
            await Task.Delay(delayms);
            if (dispatcher.HasThreadAccess) { action(); }
            else
            {
                var tcs = new TaskCompletionSource<object>();
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    try { action(); tcs.TrySetResult(null); }
                    catch (Exception ex) { tcs.TrySetException(ex); }
                });
                await tcs.Task;
            }
        }

        public async Task DispatchAsync(Func<Task> func, int delayms = 0)
        {
            await Task.Delay(delayms);
            if (dispatcher.HasThreadAccess) { await func?.Invoke(); }
            else
            {
                var tcs = new TaskCompletionSource<object>();
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    try { await func(); tcs.TrySetResult(null); }
                    catch (Exception ex) { tcs.TrySetException(ex); }
                });
                await tcs.Task;
            }
        }

        public async Task<T> DispatchAsync<T>(Func<T> func, int delayms = 0)
        {
            await Task.Delay(delayms);
            if (dispatcher.HasThreadAccess) { return func(); }
            else
            {
                var tcs = new TaskCompletionSource<T>();
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    try { tcs.TrySetResult(func()); }
                    catch (Exception ex) { tcs.TrySetException(ex); }
                });
                await tcs.Task;
                return tcs.Task.Result;
            }
        }

        public async void Dispatch(Action action, int delayms = 0)
        {
            await Task.Delay(delayms);
            if (dispatcher.HasThreadAccess) { action(); }
            else
            {
                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action()).AsTask().Wait();
            }
        }

        public T Dispatch<T>(Func<T> action, int delayms = 0) where T : class
        {
            Task.Delay(delayms);
            if (dispatcher.HasThreadAccess) { return action(); }
            else
            {
                T result = null;
                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => result = action()).AsTask().Wait();
                return result;
            }
        }
    }
}
