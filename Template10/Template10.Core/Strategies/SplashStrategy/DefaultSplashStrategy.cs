using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Template10
{
    public class SplashScreenDataContext
    {
        public SplashScreen SplashScreen { get; set; }
    }
}

namespace Template10.Strategies
{
    public static partial class Settings
    {
        public static bool ShowExtendedSplashScreen { get; set; } = true;
    }

    public interface ISplashStrategy
    {
        DataTemplate DataTemplate { get; set; }
        bool ShowSplash(SplashScreen splashScreen);
        bool HideSplash();
    }

    public class DefaultSplashStrategy : ISplashStrategy
    {
        public DataTemplate DataTemplate { get; set; }
        public bool ShowSplash(SplashScreen splashScreen)
        {
            if (Settings.ShowExtendedSplashScreen && DataTemplate != null)
            {
                // DataContext = splashScreen;
            }
            return false;
        }

        public bool HideSplash()
        {
            return false;
        }
    }
}
