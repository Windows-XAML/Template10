using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultiplePageHeaders.Views
{
    public sealed partial class ContainerPage : Page
    {
        public ContainerPage()
        {
            this.InitializeComponent();
        }

        private async void Frame1_Loaded(object sender, RoutedEventArgs e)
        {
            var frame = sender as Frame;
            var current = Template10.Common.BootStrapper.Current;
            var nav = current.NavigationServiceFactory(Template10.Common.BootStrapper.BackButton.Ignore, Template10.Common.BootStrapper.ExistingContent.Exclude, frame);
            await nav.NavigateAsync(typeof(DetailPage), "Frame 1");
        }

        private async void Frame2_Loaded(object sender, RoutedEventArgs e)
        {
            var frame = sender as Frame;
            var current = Template10.Common.BootStrapper.Current;
            var nav = current.NavigationServiceFactory(Template10.Common.BootStrapper.BackButton.Ignore, Template10.Common.BootStrapper.ExistingContent.Exclude, frame);
            await nav.NavigateAsync(typeof(DetailPage), "Frame 2");
        }
    }
}
