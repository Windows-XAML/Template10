using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Template10.Services.Dispatcher
{
    internal class DispatcherService : IDispatcherService
    {
        CoreDispatcher _dispatcher;
        public DispatcherService(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async void Dispatch(Action action)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action));
        }

        public async Task DispatchAsync(Action action)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action));
        }
    }

}