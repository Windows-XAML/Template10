using Windows.UI.Core;

namespace Template10.Common
{
    public static class DispatcherWrapperFactory
    {
        public static IDispatcherWrapper Create(CoreDispatcher dispatcher)
        {
            return new DispatcherWrapper(dispatcher);
        }
    }
}
