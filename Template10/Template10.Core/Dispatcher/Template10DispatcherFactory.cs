using Windows.UI.Core;

namespace Template10.Dispatcher
{
    public class Template10DispatcherFactory
    {
        public ITemplate10Dispatcher Create(CoreDispatcher dispatcher)
            => new Template10Dispatcher(dispatcher);
    }
}
