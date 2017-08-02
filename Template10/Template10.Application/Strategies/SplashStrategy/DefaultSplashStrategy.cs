using System;
using Template10.Services.PopupService;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Template10.Strategies.SplashStrategy
{
    public class DefaultSplashStrategy : ISplashStrategy
    {
        private Popup popup;
        private PopupService service;

        internal DefaultSplashStrategy()
        {
            service = new PopupService();
        }

        public void Show(SplashScreen splashScreen)
        {
            if (Settings.SplashFactory == null)
                return;
            var splash = Settings.SplashFactory(splashScreen);
            splash.Loaded += (s, e) =>
            {
                Window.Current.Content = splash;
                popup = service.Show(PopupSize.FullScreen, splash);
                Window.Current.Activate();
            };
        }

        public void Hide() => popup?.Hide();
        public bool IsSplashVisible => popup?.IsOpen ?? false;
    }
}
