using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Template10.App
{
    internal sealed class SplashLogic
    {
        //private Popup popup;

        //public void Show(SplashScreen splashScreen, Func<SplashScreen, UserControl> splashFactory, WindowLogic windowLogic)
        //{
        //    if (splashFactory == null)
        //        return;
        //    var splash = splashFactory(splashScreen);
        //    var service = new PopupService();
        //    popup = service.Show(PopupService.PopupSize.FullScreen, splash);
        //    windowLogic.ActivateWindow(WindowLogic.ActivateWindowSources.SplashScreen, this);
        //}

        //public void Hide()
        //{
        //    popup?.Hide();
        //}

        //public bool Splashing => popup?.IsOpen ?? false;
    }

    internal sealed class WindowLogic
    {
        //public enum ActivateWindowSources { Launching, Activating, SplashScreen, Resuming }


        //public void Activate(ActivateWindowSources source, SplashLogic splashLogic)
        //{
        //    DebugWrite($"source:{source}");

        //    if (source != ActivateWindowSources.SplashScreen)
        //    {
        //        splashLogic.Hide();
        //    }

        //    Window.Current.Activate();
        //}
    }
}
