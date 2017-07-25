using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Template10.Dispatcher
{
    public class Template10Dispatcher : ITemplate10Dispatcher
    {
        private CoreDispatcher _dispatcher;

        internal Template10Dispatcher(CoreDispatcher dispatcher)
            => _dispatcher = dispatcher;

        public async void Dispatch(Action action, int millisecond = 0)
            => await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());

        public async Task DispatchAsync(Func<Task> action, int millisecond = 0)
            => await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
    }
}
