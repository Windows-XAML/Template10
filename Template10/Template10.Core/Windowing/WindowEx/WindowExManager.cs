using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

namespace Template10.Common
{
    public static class WindowExManager
    {
        public static IList<IWindowEx> Instances { get; } = new List<IWindowEx>();

        public static IWindowEx Current()
            => WindowExManager.Instances.FirstOrDefault(x => x.Window == Window.Current) ?? GetDefault();

        public static IWindowEx Current(Window window)
            => WindowExManager.Instances.FirstOrDefault(x => x.Window == window);

        public static IWindowEx Main()
            => WindowExManager.Instances.FirstOrDefault(x => x.IsMainView);

        private static IWindowEx GetDefault()
        {
            try
            {
                // this cannot be called from a background thread.
                var mainDispatcher = CoreApplication.MainView.Dispatcher;
                return WindowExManager.Instances.FirstOrDefault(x => x.Window.Dispatcher == mainDispatcher) ??
                        WindowExManager.Instances.FirstOrDefault();
            }
            catch (COMException)
            {
                // MainView might exist but still be not accessible
                return WindowExManager.Instances.FirstOrDefault();
            }
        }

    }
}
