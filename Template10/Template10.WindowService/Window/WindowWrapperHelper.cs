using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

namespace Template10.Services.WindowWrapper
{
    public static class WindowWrapperHelper
    {
        public static List<IWindowWrapper> Instances { get; } = new List<IWindowWrapper>();

        public static IWindowWrapper Default()
        {
            try
            {
                // this cannot be called from a background thread.
                var mainDispatcher = CoreApplication.MainView.Dispatcher;
                return WindowWrapperHelper.Instances.FirstOrDefault(x => x.Window.Dispatcher == mainDispatcher) ??
                        WindowWrapperHelper.Instances.FirstOrDefault();
            }
            catch (COMException)
            {
                //MainView might exist but still be not accessible
                return WindowWrapperHelper.Instances.FirstOrDefault();
            }
        }

        public static IWindowWrapper Current() => Instances.FirstOrDefault(x => x.Window == Window.Current) ?? Default();

        public static IWindowWrapper Current(Window window) => Instances.FirstOrDefault(x => x.Window == window);

        // (should) only be called from the Application.WindowCreated event
        public static async void Create(WindowCreatedEventArgs args)
        {
            await CreateAsync(args.Window);
        }

        // this is async for future use
        public static async Task<IWindowWrapper> CreateAsync(Window window)
        {
            if (Instances.Any(x => x.Window.Equals(window)))
            {
                throw new Exception("Windows already has a wrapper; use Current(window) to fetch.");
            }
            var wrapper = new WindowWrapper(window);
            Instances.Add(wrapper);
            return wrapper;
        }
    }
}