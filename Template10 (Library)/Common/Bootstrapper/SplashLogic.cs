using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.PopupService;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Template10.Common
{
    public class SplashLogic
    {
        internal SplashLogic()
        {

        }

        private Popup popup;

        public void Show(SplashScreen splashScreen, Func<SplashScreen, UserControl> splashFactory, WindowLogic windowLogic)
        {
            if (splashFactory == null)
                return;
            var splash = splashFactory(splashScreen);
            var service = new PopupService();
            popup = service.Show(PopupService.PopupSize.FullScreen, splash);
            windowLogic.ActivateWindow(ActivateWindowSources.SplashScreen, this);
        }

        public void Hide()
        {
            popup?.Hide();
        }

        public bool Splashing => popup?.IsOpen ?? false;
    }
}
