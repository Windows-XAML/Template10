using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

namespace Template10.Services.WindowWrapper
{
    public static class WindowWrapperManager
    {
        public static List<IWindowWrapper> Instances { get; } = new List<IWindowWrapper>();

        public static IWindowWrapper Default()
        {
            try
            {
                // this cannot be called from a background thread.
                var mainDispatcher = CoreApplication.MainView.Dispatcher;
                return WindowWrapperManager.Instances.FirstOrDefault(x => x.Window.Dispatcher == mainDispatcher) ??
                        WindowWrapperManager.Instances.FirstOrDefault();
            }
            catch (COMException)
            {
                //MainView might exist but still be not accessible
                return WindowWrapperManager.Instances.FirstOrDefault();
            }
        }

        public static IWindowWrapper Current() => Instances.FirstOrDefault(x => x.Window == Window.Current) ?? Default();

        public static IWindowWrapper Current(Window window) => Instances.FirstOrDefault(x => x.Window == window);

        public static IWindowWrapper Main() => Instances.FirstOrDefault(x => x.IsMainView);
    }
}
