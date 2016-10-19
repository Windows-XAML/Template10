using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using Template10.Interfaces.Services.View;
using Windows.UI.Xaml;
using Template10.Interfaces.Services.Dispatcher;

namespace Template10.Services.Dispatcher
{

    public class DispatcherService : IDispatcherService
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