using Template10.Core;
using Windows.UI.Xaml;

namespace Template10.Extensions
{
    public static class Template10Extensions
    {
        public static ITemplate10Window Create(this WindowCreatedEventArgs args)
        {
            return Template10Window.Create(args);
        }
    }
}
