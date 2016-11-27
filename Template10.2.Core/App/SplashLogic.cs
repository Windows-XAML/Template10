using System;
using System.Linq;
using System.Collections.Generic;
using Template10.Services.Lifetime;
using Template10.Services.Overlay;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Template10.App
{
    internal sealed class SplashLogic
    {
        private Popup popup;

        public void Show(SplashScreen splashScreen, Func<SplashScreen, UserControl> splashFactory, WindowLogic window)
        {
            if (splashFactory == null)
                return;
            var splash = splashFactory(splashScreen);
            var service = new OverlayService();
            popup = service.Show(Sizes.FullScreen, splash);
            window.Activate(WindowLogic.ActivateSources.SplashScreen, this);
        }

        public void Hide()
        {
            popup?.Hide();
        }

        public bool Splashing => popup?.IsOpen ?? false;
    }

}