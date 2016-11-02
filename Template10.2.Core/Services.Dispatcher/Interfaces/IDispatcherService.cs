using System;
using System.Threading.Tasks;

namespace Template10.Services.Dispatcher
{

    public interface IDispatcherService
    {
        void Dispatch(Action action);
        Task DispatchAsync(Action action);
    }

}