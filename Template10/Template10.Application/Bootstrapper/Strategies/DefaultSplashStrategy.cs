using System;
using Template10.Services.PopupService;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Template10.Common
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
            if (SplashFactory == null)
                return;
            var splash = SplashFactory(splashScreen);
            popup = service.Show(PopupService.PopupSize.FullScreen, splash);
            Settings.WindowStrategy.ActivateWindow(ActivateWindowSources.SplashScreen);
        }

        public void Hide() => popup?.Hide();
        public bool IsSplashVisible => popup?.IsOpen ?? false;
        public Func<SplashScreen, UserControl> SplashFactory { get; set; }
    }
}
