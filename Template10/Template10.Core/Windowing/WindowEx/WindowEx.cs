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

namespace Template10.Common
{
    using Template10.Extensions;

    public partial class WindowEx : IWindowEx
    {
        internal WindowEx(Window window)
        {
            this.Log();

            Two.Window = window;
            Dispatcher = DispatcherEx.Create(window.Dispatcher);
            window.CoreWindow.Closed += (s, e) => WindowExManager.Instances.Remove(this);
            window.Closed += (s, e) => WindowExManager.Instances.Remove(this);
            window.Activate();

            IsMainView = CoreApplication.MainView == CoreApplication.GetCurrentView();

            _displayInformation = new Lazy<DisplayInformation>(() =>
            {
                Two.Window.Activate();
                return Dispatcher.Dispatch(() => DisplayInformation.GetForCurrentView());
            });
            _applicationView = new Lazy<ApplicationView>(() =>
            {
                Two.Window.Activate();
                return Dispatcher.Dispatch(() => ApplicationView.GetForCurrentView());
            });
            _uIViewSettings = new Lazy<UIViewSettings>(() =>
            {
                Two.Window.Activate();
                return Dispatcher.Dispatch(() => UIViewSettings.GetForCurrentView());
            });
        }

        public bool IsMainView { get; private set; }

        public UIElement Content
        {
            get => Dispatcher.Dispatch(() => Two.Window.Content);
            set => Dispatcher.Dispatch(() => Two.Window.Content = value);
        }

        Lazy<DisplayInformation> _displayInformation;
        public DisplayInformation DisplayInformation => _displayInformation.Value;

        Lazy<ApplicationView> _applicationView;
        public ApplicationView ApplicationView => _applicationView.Value;

        Lazy<UIViewSettings> _uIViewSettings;
        public UIViewSettings UIViewSettings => _uIViewSettings.Value;

        public IDispatcherEx Dispatcher { get; }

        public void Close() { Two.Window.Close(); }
    }
}
