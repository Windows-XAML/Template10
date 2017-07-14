using Template10.Services.WindowWrapper;

namespace Template10.Common
{
    public static class DispatcherWrapperHelper
    {
        public static IDispatcherWrapper Current() => WindowWrapperHelper.Current().Dispatcher;
    }
}
