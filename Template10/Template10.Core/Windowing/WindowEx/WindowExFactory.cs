using System;
using System.Linq;
using Windows.UI.Xaml;

namespace Template10.Common
{
    public static class WindowExFactory
    {
        public static IWindowEx Create(WindowCreatedEventArgs args)
        {
            return Create(args.Window);
        }

        public static IWindowEx Create(Window window)
        {
            if (WindowExManager.Instances.Any(x => x.Window.Equals(window)))
            {
                throw new Exception("Windows already has a wrapper; use Current(window) to fetch.");
            }
            var wrapper = new WindowEx(window);
            WindowExManager.Instances.Add(wrapper);
            ViewService.OnWindowCreated();
            return wrapper;
        }
    }
}
