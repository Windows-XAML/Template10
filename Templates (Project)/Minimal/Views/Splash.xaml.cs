using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Minimal.Views
{
    public sealed partial class Splash : Page
    {
        public Splash(SplashScreen splashScreen)
        {
            this.InitializeComponent();

            Action resize = () =>
            {
                MyImage.Height = splashScreen.ImageLocation.Height;
                MyImage.Width = splashScreen.ImageLocation.Width;
                MyImage.SetValue(Canvas.TopProperty, splashScreen.ImageLocation.Top);
                MyImage.SetValue(Canvas.LeftProperty, splashScreen.ImageLocation.Left);
            };
            Window.Current.SizeChanged += (s, e) => resize();
            resize();
        }
    }
}
