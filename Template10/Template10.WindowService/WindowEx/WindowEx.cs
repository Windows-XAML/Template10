using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Template10.Core
{
    using Template10.Services.Logging;

    public partial class WindowEx : IWindowEx2
    {
        IWindowEx2 Two => this as IWindowEx2;

        Window IWindowEx2.Window { get; set; }
    }

    public partial class WindowEx : Loggable, IWindowEx
    {
        public static IWindowEx Create(WindowCreatedEventArgs args)
        {
            return Create(args.Window);
        }

        public static IWindowEx Create(Window window)
        {
            if (Instances.Any(x => x.Window.Equals(window)))
            {
                throw new Exception("Windows already has a wrapper; use Current(window) to fetch.");
            }
            var wrapper = new WindowEx(window);
            Instances.Add(wrapper);
            ViewService.OnWindowCreated();
            return wrapper;
        }

        private WindowEx(Window window)
        {
            LogThis();

            Two.Window = window;
            Dispatcher = DispatcherEx.Create(window.Dispatcher);
            window.CoreWindow.Closed += (s, e) => Instances.Remove(this);
            window.Closed += (s, e) => Instances.Remove(this);
            window.Activate();

            IsMainView = CoreApplication.MainView == CoreApplication.GetCurrentView();

            _DisplayInformation = new Lazy<DisplayInformation>(() =>
            {
                Two.Window.Activate();
                return Dispatcher.Dispatch(() => DisplayInformation.GetForCurrentView());
            });
            _ApplicationView = new Lazy<ApplicationView>(() =>
            {
                Two.Window.Activate();
                return Dispatcher.Dispatch(() => ApplicationView.GetForCurrentView());
            });
            _UIViewSettings = new Lazy<UIViewSettings>(() =>
            {
                Two.Window.Activate();
                return Dispatcher.Dispatch(() => UIViewSettings.GetForCurrentView());
            });
        }

        public static List<IWindowEx> Instances { get; } = new List<IWindowEx>();

        public static IWindowEx GetDefault()
        {
            try
            {
                // this cannot be called from a background thread.
                var mainDispatcher = CoreApplication.MainView.Dispatcher;
                return WindowEx.Instances.FirstOrDefault(x => x.Window.Dispatcher == mainDispatcher) ??
                        WindowEx.Instances.FirstOrDefault();
            }
            catch (COMException)
            {
                // MainView might exist but still be not accessible
                return WindowEx.Instances.FirstOrDefault();
            }
        }

        public static IWindowEx Current() => Instances.FirstOrDefault(x => x.Window == Window.Current) ?? GetDefault();

        public static IWindowEx Current(Window window) => Instances.FirstOrDefault(x => x.Window == window);

        public static IWindowEx Main() => Instances.FirstOrDefault(x => x.IsMainView);

        public bool IsMainView { get; private set; }

        public UIElement Content
        {
            get => Dispatcher.Dispatch(() => Two.Window.Content);
            set => Dispatcher.Dispatch(() => Two.Window.Content = value);
        }

        Lazy<DisplayInformation> _DisplayInformation;
        public DisplayInformation DisplayInformation => _DisplayInformation.Value;

        Lazy<ApplicationView> _ApplicationView;
        public ApplicationView ApplicationView => _ApplicationView.Value;

        Lazy<UIViewSettings> _UIViewSettings;
        public UIViewSettings UIViewSettings => _UIViewSettings.Value;

        public IDispatcherEx Dispatcher { get; }

        public void Close() { Two.Window.Close(); }
    }
}
