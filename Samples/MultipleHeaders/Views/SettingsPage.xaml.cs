using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MultipleHeaders.Views
{
    public sealed partial class SettingsPage : Page
    {
        Template10.Services.SerializationService.ISerializationService _SerializationService;

        public SettingsPage()
        {
            InitializeComponent();
            _SerializationService = Template10.Services.SerializationService.SerializationService.Json;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var index = int.Parse(_SerializationService.Deserialize(e.Parameter?.ToString()).ToString());
            MyPivot.SelectedIndex = index;
        }

        private void MyFrame_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //var current = Template10.Common.BootStrapper.Current;
            //var nav = current.NavigationServiceFactory(Template10.Common.BootStrapper.BackButton.Ignore, Template10.Common.BootStrapper.ExistingContent.Include, sender as Frame);
            //nav.Navigate(typeof(MainPage));
        }
    }
}
