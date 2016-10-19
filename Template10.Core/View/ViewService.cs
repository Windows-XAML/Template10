using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Template10.View
{
    public interface IDispatcherService
    {
        void Dispatch(Action action);
        Task DispatchAsync(Action action);
    }

    public class DispatcherService : IDispatcherService
    {
        CoreDispatcher _dispatcher;
        public DispatcherService(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async void Dispatch(Action action)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action));
        }

        public async Task DispatchAsync(Action action)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action));
        }
    }

    internal class ViewService : IViewService
    {
        public ViewService(Window window)
        {
            Window = window;
            DispatcherService = new DispatcherService(window.Dispatcher);
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

        public List<Navigation.INavigationService> NavigationServices { get; } = new List<Navigation.INavigationService>();

        #region Static

        private static List<IViewService> ActiveWindows = new List<IViewService>();

        public static IEnumerable<Navigation.INavigationService> AllNavigationServices
            => ActiveWindows.SelectMany(x => x.NavigationServices);

        public static IViewService Main()
            => ActiveWindows.FirstOrDefault(x => x.IsInMainView) ?? null;

        public static IViewService Current()
            => ActiveWindows.FirstOrDefault(x => x.Window.Equals(Current())) ?? Main();

        public static IViewService Current(Windows.UI.Xaml.Window window)
            => ActiveWindows.FirstOrDefault(x => x.Window == window);

        public static IViewService Current(Template10.Navigation.INavigationService nav)
            => ActiveWindows.FirstOrDefault(x => x.NavigationServices.Contains(nav));

        #endregion  
    }
}