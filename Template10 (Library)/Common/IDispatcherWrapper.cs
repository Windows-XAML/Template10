using System;
using System.Threading.Tasks;

namespace Template10.Common
{
    using Windows.UI.Core;

    public interface IDispatcherWrapper
    {
        void Dispatch(Action action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal);
        Task DispatchAsync(Func<Task> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal);
        Task DispatchAsync(Action action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal);
        Task<T> DispatchAsync<T>(Func<T> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal);

        bool HasThreadAccess();
    }
}