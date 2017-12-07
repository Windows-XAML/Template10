using Template10.Common;
using Windows.UI.Xaml;

namespace Template10.Extensions
{
    public static class WindowExtensions
    {
        public static IWindowEx Create(this WindowCreatedEventArgs args)
        {
            return WindowExFactory.Create(args);
        }
    }
}
