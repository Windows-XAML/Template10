using Template10.Services.Serialization;
using Template10.Extensions;
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

        public ViewModels.SettingsPageViewModel ViewModel
            => DataContext as ViewModels.SettingsPageViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string s && s != null)
            {
                var serial = Template10.Central.Serialization;
                var parameter = serial.Deserialize().ToString();
                if (int.TryParse(parameter, out var index))
                {
                    MyPivot.SelectedIndex = index;
                }
            }
        }
    }

    public class DocSection
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}