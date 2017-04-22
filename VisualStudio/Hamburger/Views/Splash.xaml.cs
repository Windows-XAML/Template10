using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class Splash : UserControl
    {
        public Splash(SplashScreen splashScreen)
        {
            InitializeComponent();
            Window.Current.SizeChanged += (s, e) => Resize(splashScreen);
            Resize(splashScreen);
            Opacity = 0;
        }

        private void Resize(SplashScreen splashScreen)
        {
            if (splashScreen.ImageLocation.Top == 0)
            {
                splashImage.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                rootCanvas.Background = null;
                splashImage.Visibility = Visibility.Visible;
            }
            splashImage.Height = splashScreen.ImageLocation.Height;
            splashImage.Width = splashScreen.ImageLocation.Width;
            splashImage.SetValue(Canvas.TopProperty, splashScreen.ImageLocation.Top);
            splashImage.SetValue(Canvas.LeftProperty, splashScreen.ImageLocation.Left);
            ProgressTransform.TranslateY = splashImage.Height / 2;
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            Opacity = 1;
        }
    }
}
