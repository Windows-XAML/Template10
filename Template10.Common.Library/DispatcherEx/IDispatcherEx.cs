using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Template10.Common
{
    public interface IDispatcherEx 
    {
        bool HasThreadAccess();
        void Dispatch(Action action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal);
        T Dispatch<T>(Func<T> action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal);
        Task DispatchAsync(Func<Task> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal);
        Task<T> DispatchAsync<T>(Func<Task<T>> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal);
        Task DispatchAsync(Action action, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal);
        Task<T> DispatchAsync<T>(Func<T> func, int delayms = 0, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal);
    }
}