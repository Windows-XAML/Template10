using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class Splash : UserControl
    {
        public SplashScreen SplashScreen { get; set; }

        public Splash()
        {
            // empty
        }

        public Splash(SplashScreen splashScreen)
        {
            DataContext = splashScreen;
            InitializeComponent();
            Window.Current.SizeChanged += (s, e) => Resize();
            Resize();
            Opacity = 0;
        }

        private void Resize()
        {
            if (SplashScreen.ImageLocation.Top == 0)
            {
                splashImage.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                rootCanvas.Background = null;
                splashImage.Visibility = Visibility.Visible;
            }
            splashImage.Height = SplashScreen.ImageLocation.Height;
            splashImage.Width = SplashScreen.ImageLocation.Width;
            splashImage.SetValue(Canvas.TopProperty, SplashScreen.ImageLocation.Top);
            splashImage.SetValue(Canvas.LeftProperty, SplashScreen.ImageLocation.Left);
            ProgressTransform.TranslateY = splashImage.Height / 2 + 10;
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            Opacity = 1;
        }
    }
}
