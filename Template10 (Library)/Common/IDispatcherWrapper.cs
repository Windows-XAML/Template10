using System;
using System.Threading.Tasks;

namespace Template10.Common
{
    public interface IDispatcherWrapper
    {
        void Dispatch(Action action);
        Task DispatchAsync(Func<Task> func);
        Task DispatchAsync(Action action);
        Task<T> DispatchAsync<T>(Func<T> func);
        bool HasThreadAccess();
    }
}