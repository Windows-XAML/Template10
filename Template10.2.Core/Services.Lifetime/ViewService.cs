using System.Linq;
using System.Collections.Generic;
using Template10.Interfaces.Services.Lifetime;
using Template10.Services.Navigation;
using Windows.UI.Xaml;
using Template10.Interfaces.Services.Dispatcher;

namespace Template10.Services.Lifetime
{
    internal class ViewService : IViewService
    {
        public ViewService(Window window)
        {
            Window = window;

            DispatcherService = new Dispatcher.DispatcherService(window.Dispatcher);

            DisplayInformation = Windows.Graphics.Display.DisplayInformation.GetForCurrentView();

            ApplicationView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();

            UIViewSettings = Windows.UI.ViewManagement.UIViewSettings.GetForCurrentView();

            ActiveWindows.Add(this);
        }

        public Window Window { get; }

        public IDispatcherService DispatcherService { get; }

        public Windows.Graphics.Display.DisplayInformation DisplayInformation { get; }

        public Windows.UI.ViewManagement.ApplicationView ApplicationView { get; }

        public Windows.UI.ViewManagement.UIViewSettings UIViewSettings { get; }

        public bool IsInMainView
            => Windows.ApplicationModel.Core.CoreApplication.MainView == Windows.ApplicationModel.Core.CoreApplication.GetCurrentView();

        public List<INavigationService> NavigationServices { get; } = new List<INavigationService>();

        #region Static

        private static List<IViewService> ActiveWindows = new List<IViewService>();

        public static IEnumerable<INavigationService> AllNavigationServices
            => ActiveWindows.SelectMany(x => x.NavigationServices);

        public static IViewService Main()
            => ActiveWindows.FirstOrDefault(x => x.IsInMainView) ?? null;

        public static IViewService Current()
            => ActiveWindows.FirstOrDefault(x => x.Window.Equals(Current())) ?? Main();

        public static IViewService Current(Windows.UI.Xaml.Window window)
            => ActiveWindows.FirstOrDefault(x => x.Window == window);

        public static IViewService Current(INavigationService nav)
            => ActiveWindows.FirstOrDefault(x => x.NavigationServices.Contains(nav));

        #endregion  
    }
}