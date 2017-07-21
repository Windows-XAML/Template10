using System;
using System.Linq;
using Windows.UI.Xaml;

namespace Template10.Services.WindowWrapper
{
    public static class WindowWrapperFactory
    {
        public static IWindowWrapper Create(WindowCreatedEventArgs args) => Create(args.Window);

        public static IWindowWrapper Create(Window window)
        {
            if (WindowWrapperManager.Instances.Any(x => x.Window.Equals(window)))
            {
                throw new Exception("Windows already has a wrapper; use Current(window) to fetch.");
            }
            var wrapper = new WindowWrapper(window);
            WindowWrapperManager.Instances.Add(wrapper);
            ViewService.ViewService.OnWindowCreated();
            return wrapper;
        }
    }
}
