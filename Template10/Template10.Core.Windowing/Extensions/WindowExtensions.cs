using Template10.Core;
using Windows.UI.Xaml;

namespace Template10.Extensions
{
    public static class WindowExtensions
    {
        public static IWindowEx Create(this WindowCreatedEventArgs args)
        {
            return WindowEx.Create(args);
        }
    }
}
