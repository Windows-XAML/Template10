using System;
using System.Runtime.CompilerServices;
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

        public DispatcherWrapper(CoreDispatcher dispatcher)
        {
            DebugWrite(caller: "Constructor");
            this.dispatcher = dispatcher;
        }

        public bool HasThreadAccess() => dispatcher.HasThreadAccess;

        private readonly CoreDispatcher dispatcher;

        public async Task<T> DispatchAsync<T>(Func<Task<T>> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (this.dispatcher.HasThreadAccess)
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(true);
                return await func().ConfigureAwait(false);
            }
            else
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(false);
                var tcs = new TaskCompletionSource<T>();
                dispatcher.RunAsync(priority, async delegate
                {
                    try
                    {
                        var result = await func().ConfigureAwait(false);
                        tcs.TrySetResult(result);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                });
                return await tcs.Task.ConfigureAwait(false);
            }
        }

        public async Task DispatchAsync(Action action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (this.dispatcher.HasThreadAccess)
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(true);
                action();
            }
            else
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(false);
                var tcs = new TaskCompletionSource<object>();
                dispatcher.RunAsync(priority, delegate
                {
                    try
                    {
                        action();
                        tcs.TrySetResult(null);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                });
                await tcs.Task.ConfigureAwait(false);
            }
        }

        public async Task DispatchAsync(Func<Task> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (this.dispatcher.HasThreadAccess)
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(true);
                await func().ConfigureAwait(false);
            }
            else
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(false);
                var tcs = new TaskCompletionSource<object>();
                dispatcher.RunAsync(priority, async delegate
                {
                    try
                    {
                        await func().ConfigureAwait(false);
                        tcs.TrySetResult(null);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                });
                await tcs.Task.ConfigureAwait(false);
            }
        }

        public async Task<T> DispatchAsync<T>(Func<T> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (this.dispatcher.HasThreadAccess)
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(true);
                return func();
            }
            else
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(false);
                var tcs = new TaskCompletionSource<T>();
                dispatcher.RunAsync(priority, delegate
                {
                    try
                    {
                        tcs.TrySetResult(func());
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                });
                return await tcs.Task.ConfigureAwait(false);
            }
        }

        public void Dispatch(Action action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (delayms > 0)
                Task.Delay(delayms).ConfigureAwait(false).GetAwaiter().GetResult();

            dispatcher.RunAsync(priority, () => action());
        }

        public T Dispatch<T>(Func<T> action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (this.dispatcher.HasThreadAccess)
            {
                if (delayms > 0)
                    Task.Delay(delayms).ConfigureAwait(true).GetAwaiter().GetResult();
                return action();
            }
            else
            {
                if (delayms > 0)
                    Task.Delay(delayms).ConfigureAwait(false).GetAwaiter().GetResult();
                var tcs = new TaskCompletionSource<T>();
                dispatcher.RunAsync(priority, delegate
                {
                    try
                    {
                        tcs.TrySetResult(action());
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                });
                return tcs.Task.ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        public async Task DispatchIdleAsync(Action action, int delayms = 0)
        {
            if (this.dispatcher.HasThreadAccess)
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(true);
                action();
            }
            else
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(false);
                var tcs = new TaskCompletionSource<object>();
                dispatcher.RunIdleAsync(delegate
                {
                    try
                    {
                        action();
                        tcs.TrySetResult(null);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                });
                await tcs.Task.ConfigureAwait(false);
            }
        }

        public async Task DispatchIdleAsync(Func<Task> func, int delayms = 0)
        {
            if (this.dispatcher.HasThreadAccess)
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(true);
                await func().ConfigureAwait(false);
            }
            else
            {
                if (delayms > 0)
                    await Task.Delay(delayms).ConfigureAwait(false);
                var tcs = new TaskCompletionSource<object>();
                dispatcher.RunIdleAsync(async delegate
                {
                    try
                    {
                        await func().ConfigureAwait(false);
                        tcs.TrySetResult(null);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                });
                await tcs.Task.ConfigureAwait(false);
            }
        }

        public async Task<T> DispatchIdleAsync<T>(Func<T> func, int delayms = 0)
        {
            if (delayms > 0)
                await Task.Delay(delayms).ConfigureAwait(this.dispatcher.HasThreadAccess);

            var tcs = new TaskCompletionSource<T>();
            dispatcher.RunIdleAsync(delegate
            {
                try
                {
                    tcs.TrySetResult(func());
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });
            return await tcs.Task.ConfigureAwait(false);
        }

        public void DispatchIdle(Action action, int delayms = 0)
        {
            if (delayms > 0)
                Task.Delay(delayms).ConfigureAwait(false).GetAwaiter().GetResult();

            dispatcher.RunIdleAsync(args => action());
        }

        public T DispatchIdle<T>(Func<T> action, int delayms = 0) where T : class
        {
            if (this.dispatcher.HasThreadAccess)
            {
                if (delayms > 0)
                    Task.Delay(delayms).ConfigureAwait(true).GetAwaiter().GetResult();
                return action();
            }
            else
            {
                if (delayms > 0)
                    Task.Delay(delayms).ConfigureAwait(false).GetAwaiter().GetResult();
                var tcs = new TaskCompletionSource<T>();
                dispatcher.RunIdleAsync(delegate
                {
                    try
                    {
                        tcs.TrySetResult(action());
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                });
                return tcs.Task.ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}
