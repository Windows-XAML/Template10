using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    public interface ISplashStrategy
    {
        void Hide();
        void Show(SplashScreen splashScreen);
        bool IsSplashVisible { get; }
        Func<SplashScreen, UserControl> SplashFactory { get; set; }
    }
}