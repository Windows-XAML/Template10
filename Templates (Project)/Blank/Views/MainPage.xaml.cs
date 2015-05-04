using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Blank1.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void LaunchUri(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri((sender as HyperlinkButton).Content.ToString()));
        }
    }
}
