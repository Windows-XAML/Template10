using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.WindowService
{

    public class WindowService : IWindowService
    {
        public WindowService(Windows.UI.Xaml.Window window)
        {
            Window = window;
            DisplayInformation = Windows.Graphics.Display.DisplayInformation.GetForCurrentView();
            ApplicationView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            UIViewSettings = Windows.UI.ViewManagement.UIViewSettings.GetForCurrentView();
        }

        public Windows.UI.Xaml.Window Window { get; }

        public Windows.Graphics.Display.DisplayInformation DisplayInformation { get; }

        public Windows.UI.ViewManagement.ApplicationView ApplicationView { get; }

        public Windows.UI.ViewManagement.UIViewSettings UIViewSettings { get; }

        public bool IsInMainView => Windows.ApplicationModel.Core.CoreApplication.MainView == Windows.ApplicationModel.Core.CoreApplication.GetCurrentView();

        #region Static

        public static List<Template10.Navigation.INavigationService> NavigationServices = new List<Template10.Navigation.INavigationService>();

        private static List<IWindowService> ActiveWindows = new List<IWindowService>();

        public static IWindowService Default()
        {
            try
            {
                return ActiveWindows.FirstOrDefault(x => x.Window.Dispatcher == Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher)
                    ?? ActiveWindows.FirstOrDefault();
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return ActiveWindows.FirstOrDefault();
            }
        }

        public static IWindowService Current()
            => ActiveWindows.FirstOrDefault(x => x.Window.Equals(Current())) ?? Default();

        public static IWindowService Current(Windows.UI.Xaml.Window window)
            => ActiveWindows.FirstOrDefault(x => x.Window == window);

        public static IWindowService Current(Template10.Navigation.INavigationService nav)
            => ActiveWindows.FirstOrDefault(x => NavigationServices.Contains(nav));

        #endregion  
    }
}