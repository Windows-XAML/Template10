using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Template10.Services.WindowWrapper
{

    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-WindowWrapper
    public class WindowWrapper : IWindowWrapper
    {
        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(WindowWrapper)}.{caller}");

        #endregion

        internal WindowWrapper(Window window)
        {
            Window = window;
            Dispatcher = new DispatcherWrapper(window.Dispatcher);
            window.CoreWindow.Closed += (s, e) => WindowWrapperManager.Instances.Remove(this);
            window.Closed += (s, e) => WindowWrapperManager.Instances.Remove(this);
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

        public bool IsMainView { get; private set; }

        public object Content => Dispatcher.Dispatch(() => Window.Content);

        Lazy<DisplayInformation> _DisplayInformation;
        public DisplayInformation DisplayInformation => _DisplayInformation.Value;

        Lazy<ApplicationView> _ApplicationView;
        public ApplicationView ApplicationView => _ApplicationView.Value;

        Lazy<UIViewSettings> _UIViewSettings;
        public UIViewSettings UIViewSettings => _UIViewSettings.Value;

        public Window Window { get; }

        public IDispatcherWrapper Dispatcher { get; }

        public void Close() { Window.Close(); }
    }
}
