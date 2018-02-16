using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultiplePageHeaders.Controls
{
    public sealed partial class MyUserControl : UserControl
    {
        public MyUserControl()
        {
            InitializeComponent();
        }

        static int counter = 0;

        private async void Frame_Loaded(object sender, RoutedEventArgs e)
        {
            var frame = sender as Frame;
            var current = Template10.Common.BootStrapper.Current;
            var nav = current.NavigationServiceFactory(Template10.Common.BootStrapper.BackButton.Ignore, Template10.Common.BootStrapper.ExistingContent.Exclude, frame);
            await nav.NavigateAsync(typeof(Views.DetailPage), $"Frame {++counter}");
        }
    }
}
