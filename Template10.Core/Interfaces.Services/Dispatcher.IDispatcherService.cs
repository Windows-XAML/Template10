using System;
using System.Threading.Tasks;

namespace Template10.Interfaces.Services.Dispatcher
{

    public interface IDispatcherService
    {
        void Dispatch(Action action);
        Task DispatchAsync(Action action);
    }

}