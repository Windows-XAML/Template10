using System;
using Template10.Services.Lifetime;
using Template10.Services.Overlay;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Template10.App
{
    internal sealed class WindowLogic
    {
        public IViewService Register(Window window)
        {
            return new ViewService(window);
        }

        public enum ActivateSources { Launching, Activating, SplashScreen, Resuming }

        public void Activate(ActivateSources source, SplashLogic splash = null)
        {
            this.LogInfo($"source:{source}");

            if (source != ActivateSources.SplashScreen)
            {
                splash?.Hide();
            }

            Window.Current.Activate();
        }
    }
}
