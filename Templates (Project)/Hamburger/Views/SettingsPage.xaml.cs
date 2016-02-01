using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Sample.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var index = Template10.Services.SerializationService.SerializationService
                .Json.Deserialize<int>(e.Parameter?.ToString());
            MyPivot.SelectedIndex = index;
        }
    }
}