using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    public class WindowWrapper
    {
        public readonly static List<WindowWrapper> ActiveWrappers = new List<WindowWrapper>();
        public static WindowWrapper Current() { return ActiveWrappers.First(x => x.Window.Dispatcher.HasThreadAccess); }
        public static WindowWrapper Current(Window window) { return ActiveWrappers.First(x => x.Window == window); }

        public WindowWrapper(Window window)
        {
            if (ActiveWrappers.Any(x => x.Window == window))
                throw new Exception("Windows already has a wrapper; use Current(window) to fetch.");
            Window = window;
            ActiveWrappers.Add(this);
            Dispatcher = new DispatcherWrapper(window.Dispatcher);
            window.Closed += (s, e) => { ActiveWrappers.Remove(this); };
        }

        Window Window { get; set; }
        public void Close() { Window.Close(); }
        public DispatcherWrapper Dispatcher { get; private set; }
        public List<NavigationService> NavigationServices { get; } = new List<NavigationService>();
    }
}
