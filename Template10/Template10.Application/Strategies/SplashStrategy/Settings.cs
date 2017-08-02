using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace Template10.Strategies.SplashStrategy
{
    public static class Settings
    {
        public static Func<SplashScreen, UserControl> SplashFactory { get; set; }
    }
}
