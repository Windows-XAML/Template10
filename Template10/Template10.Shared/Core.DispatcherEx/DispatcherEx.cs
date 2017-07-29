using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Core;
using Template10.Services.LoggingService;
using Windows.UI.Core;

namespace Template10.Core
{
    public class DispatcherEx : IDispatcherEx
    {
        #region Debug

        static void DebugWrite(string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null) =>
            LoggingService.WriteLine(text, severity, caller: $"DispatcherWrapper.{caller}");

        #endregion

        public static IDispatcherEx Create(CoreDispatcher dispatcher)
        {
            return new DispatcherEx(dispatcher);
        }

        private DispatcherEx(CoreDispatcher dispatcher)
        {
            Two.CoreDispatcher = dispatcher;
        }

        public bool HasThreadAccess() => Two.CoreDispatcher.HasThreadAccess;

        IDispatcherEx2 Two => this as IDispatcherEx2;

        CoreDispatcher IDispatcherEx2.CoreDispatcher { get; set; }

        public async Task<T> DispatchAsync<T>(Func<Task<T>> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (delayms > 0)
            {
                await Task.Delay(delayms).ConfigureAwait(Two.CoreDispatcher.HasThreadAccess);
            }

            if (Two.CoreDispatcher.HasThreadAccess && priority == CoreDispatcherPriority.Normal)
            {
                return await func().ConfigureAwait(false);
            }
            else
            {
                var tcs = new TaskCompletionSource<T>();
                await Two.CoreDispatcher.RunAsync(priority, async () =>
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
                }).AsTask().ConfigureAwait(false);
                return await tcs.Task.ConfigureAwait(false);
            }
        }

        public async Task DispatchAsync(Action action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (delayms > 0)
                await Task.Delay(delayms).ConfigureAwait(Two.CoreDispatcher.HasThreadAccess);

            if (Two.CoreDispatcher.HasThreadAccess && priority == CoreDispatcherPriority.Normal)
            {
                action();
            }
            else
            {
                var tcs = new TaskCompletionSource<object>();
                await Two.CoreDispatcher.RunAsync(priority, () =>
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
                }).AsTask().ConfigureAwait(false);
                await tcs.Task.ConfigureAwait(false);
            }
        }

        public async Task DispatchAsync(Func<Task> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (delayms > 0)
                await Task.Delay(delayms).ConfigureAwait(Two.CoreDispatcher.HasThreadAccess);

            if (Two.CoreDispatcher.HasThreadAccess && priority == CoreDispatcherPriority.Normal)
            {
                await func().ConfigureAwait(false);
            }
            else
            {
                var tcs = new TaskCompletionSource<object>();
                await Two.CoreDispatcher.RunAsync(priority, async () =>
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
                }).AsTask().ConfigureAwait(false);
                await tcs.Task.ConfigureAwait(false);
            }
        }

        public async Task<T> DispatchAsync<T>(Func<T> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (delayms > 0)
                await Task.Delay(delayms).ConfigureAwait(Two.CoreDispatcher.HasThreadAccess);

            if (Two.CoreDispatcher.HasThreadAccess && priority == CoreDispatcherPriority.Normal)
            {
                return func();
            }
            else
            {
                var tcs = new TaskCompletionSource<T>();
                await Two.CoreDispatcher.RunAsync(priority, () =>
                {
                    try
                    {
                        tcs.TrySetResult(func());
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }).AsTask().ConfigureAwait(false);
                return await tcs.Task.ConfigureAwait(false);
            }
        }

        public void Dispatch(Action action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (delayms > 0)
                Task.Delay(delayms).ConfigureAwait(Two.CoreDispatcher.HasThreadAccess).GetAwaiter().GetResult();

            if (Two.CoreDispatcher.HasThreadAccess && priority == CoreDispatcherPriority.Normal)
            {
                action();
            }
            else
            {
                Two.CoreDispatcher.RunAsync(priority, () => action()).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        public T Dispatch<T>(Func<T> action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (delayms > 0)
                Task.Delay(delayms).ConfigureAwait(Two.CoreDispatcher.HasThreadAccess).GetAwaiter().GetResult();

            if (Two.CoreDispatcher.HasThreadAccess && priority == CoreDispatcherPriority.Normal)
            {
                return action();
            }
            else
            {
                var tcs = new TaskCompletionSource<T>();
                Two.CoreDispatcher.RunAsync(priority, delegate
                {
                    try
                    {
                        tcs.TrySetResult(action());
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                return tcs.Task.ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        public async Task DispatchIdleAsync(Action action, int delayms = 0)
        {
            if (delayms > 0)
                await Task.Delay(delayms).ConfigureAwait(false);

            var tcs = new TaskCompletionSource<object>();
            await Two.CoreDispatcher.RunIdleAsync(delegate
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
            }).AsTask().ConfigureAwait(false);
            await tcs.Task.ConfigureAwait(false);
        }

        public async Task DispatchIdleAsync(Func<Task> func, int delayms = 0)
        {
            if (delayms > 0)
                await Task.Delay(delayms).ConfigureAwait(false);

            var tcs = new TaskCompletionSource<object>();
            await Two.CoreDispatcher.RunIdleAsync(async delegate
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
            }).AsTask().ConfigureAwait(false);
            await tcs.Task.ConfigureAwait(false);
        }

        public async Task<T> DispatchIdleAsync<T>(Func<T> func, int delayms = 0)
        {
            if (delayms > 0)
                await Task.Delay(delayms).ConfigureAwait(false);

            var tcs = new TaskCompletionSource<T>();
            await Two.CoreDispatcher.RunIdleAsync(delegate
            {
                try
                {
                    tcs.TrySetResult(func());
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }).AsTask().ConfigureAwait(false);
            return await tcs.Task.ConfigureAwait(false);
        }

        public void DispatchIdle(Action action, int delayms = 0)
        {
            if (delayms > 0)
                Task.Delay(delayms).ConfigureAwait(false).GetAwaiter().GetResult();

            Two.CoreDispatcher.RunIdleAsync(args => action()).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public T DispatchIdle<T>(Func<T> action, int delayms = 0) where T : class
        {
            if (delayms > 0)
                Task.Delay(delayms).ConfigureAwait(false).GetAwaiter().GetResult();

            var tcs = new TaskCompletionSource<T>();
            Two.CoreDispatcher.RunIdleAsync(delegate
            {
                try
                {
                    tcs.TrySetResult(action());
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            return tcs.Task.ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
