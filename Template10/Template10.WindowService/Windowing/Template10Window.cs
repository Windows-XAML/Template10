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
    public class Template10Window : ITemplate10Window
    {
        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(Template10Window)}.{caller}");

        #endregion

        public static ITemplate10Window Create(WindowCreatedEventArgs args) => Create(args.Window);

        public static ITemplate10Window Create(Window window)
        {
            if (Instances.Any(x => x.Window.Equals(window)))
            {
                throw new Exception("Windows already has a wrapper; use Current(window) to fetch.");
            }
            var wrapper = new Template10Window(window);
            Instances.Add(wrapper);
            ViewService.OnWindowCreated();
            return wrapper;
        }

        private Template10Window(Window window)
        {
            Window = window;
            Dispatcher = Template10Dispatcher.Create(window.Dispatcher);
            window.CoreWindow.Closed += (s, e) => Instances.Remove(this);
            window.Closed += (s, e) => Instances.Remove(this);
            window.Activate();

            IsMainView = CoreApplication.MainView == CoreApplication.GetCurrentView();

            _DisplayInformation = new Lazy<DisplayInformation>(() =>
            {
                Window.Activate();
                return Dispatcher.Dispatch(() => DisplayInformation.GetForCurrentView());
            });
            _ApplicationView = new Lazy<ApplicationView>(() =>
            {
                Window.Activate();
                return Dispatcher.Dispatch(() => ApplicationView.GetForCurrentView());
            });
            _UIViewSettings = new Lazy<UIViewSettings>(() =>
            {
                Window.Activate();
                return Dispatcher.Dispatch(() => UIViewSettings.GetForCurrentView());
            });
        }

        public static List<ITemplate10Window> Instances { get; } = new List<ITemplate10Window>();

        public static ITemplate10Window Default()
        {
            try
            {
                // this cannot be called from a background thread.
                var mainDispatcher = CoreApplication.MainView.Dispatcher;
                return Template10Window.Instances.FirstOrDefault(x => x.Window.Dispatcher == mainDispatcher) ??
                        Template10Window.Instances.FirstOrDefault();
            }
            catch (COMException)
            {
                //MainView might exist but still be not accessible
                return Template10Window.Instances.FirstOrDefault();
            }
        }

        public static ITemplate10Window Current() => Instances.FirstOrDefault(x => x.Window == Window.Current) ?? Default();

        public static ITemplate10Window Current(Window window) => Instances.FirstOrDefault(x => x.Window == window);

        public static ITemplate10Window Main() => Instances.FirstOrDefault(x => x.IsMainView);

        public bool IsMainView { get; private set; }

        public UIElement Content
        {
            get => Dispatcher.Dispatch(() => Window.Content);
            set => Dispatcher.Dispatch(() => Window.Content = value);
        }

        Lazy<DisplayInformation> _DisplayInformation;
        public DisplayInformation DisplayInformation => _DisplayInformation.Value;

        Lazy<ApplicationView> _ApplicationView;
        public ApplicationView ApplicationView => _ApplicationView.Value;

        Lazy<UIViewSettings> _UIViewSettings;
        public UIViewSettings UIViewSettings => _UIViewSettings.Value;

        public Window Window { get; }

        public ITemplate10Dispatcher Dispatcher { get; }

        public void Close() { Window.Close(); }
    }
}
