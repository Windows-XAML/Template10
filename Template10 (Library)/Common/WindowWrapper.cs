using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.ApplicationModel.Core;
using Template10.Services.NavigationService;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Template10.Common
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-WindowWrapper
    public class WindowWrapper
    {
        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"WindowWrapper.{caller}");

        #endregion

        static WindowWrapper()
        {
            DebugWrite(caller: "Static Constructor");
        }

        public WindowWrapper()
        {
            DebugWrite(caller: "Constructor");
        }

        public static WindowWrapper Default()
        {
            try
            {
                var mainDispatcher = CoreApplication.MainView.Dispatcher;
                return ActiveWrappers.FirstOrDefault(x => x.Window.Dispatcher == mainDispatcher) ??
                        ActiveWrappers.FirstOrDefault();
            }
            catch (COMException)
            {
                //MainView might exist but still be not accessible
                return ActiveWrappers.FirstOrDefault();
            }
        }

        public object Content => Dispatcher.Dispatch(() => Window.Content);

        public readonly static List<WindowWrapper> ActiveWrappers = new List<WindowWrapper>();

        public static WindowWrapper Current() => ActiveWrappers.FirstOrDefault(x => x.Window == Window.Current) ?? Default();

        public static WindowWrapper Current(Window window) => ActiveWrappers.FirstOrDefault(x => x.Window == window);

        public static WindowWrapper Current(INavigationService nav) => ActiveWrappers.FirstOrDefault(x => x.NavigationServices.Contains(nav));

        public DisplayInformation DisplayInformation() => Dispatcher.Dispatch(() => Windows.Graphics.Display.DisplayInformation.GetForCurrentView());

        public ApplicationView ApplicationView() => Dispatcher.Dispatch(() => Windows.UI.ViewManagement.ApplicationView.GetForCurrentView());

        public UIViewSettings UIViewSettings() => Dispatcher.Dispatch(() => Windows.UI.ViewManagement.UIViewSettings.GetForCurrentView());

        internal WindowWrapper(Window window)
        {
            if (Current(window) != null)
            {
                throw new Exception("Windows already has a wrapper; use Current(window) to fetch.");
            }
            Window = window;
            ActiveWrappers.Add(this);
            Dispatcher = new DispatcherWrapper(window.Dispatcher);
            window.CoreWindow.Closed += (s, e) =>
            {
                ActiveWrappers.Remove(this);
            };
            window.Closed += (s, e) =>
            {
                ActiveWrappers.Remove(this);
            };
        }

        public void Close() { Window.Close(); }
        public Window Window { get; }
        public DispatcherWrapper Dispatcher { get; }
        public NavigationServiceList NavigationServices { get; } = new NavigationServiceList();
    }
}
