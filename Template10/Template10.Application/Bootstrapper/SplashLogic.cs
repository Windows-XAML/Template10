using Template10.Services.PopupService;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls.Primitives;

namespace Template10.Common
{
    public class SplashLogic : ISplashLogic
    {
        internal SplashLogic()
        {

        }

        private Popup popup;

        public void Show(SplashScreen splashScreen, IBootStrapper bootstrapper)
        {
            if (bootstrapper.SplashFactory == null)
                return;
            var splash = bootstrapper.SplashFactory(splashScreen);
            var service = new PopupService();
            popup = service.Show(PopupService.PopupSize.FullScreen, splash);
            bootstrapper.WindowLogic.ActivateWindow(ActivateWindowSources.SplashScreen, this);
        }

        public void Hide()
        {
            popup?.Hide();
        }

        public bool Splashing => popup?.IsOpen ?? false;
    }
}
