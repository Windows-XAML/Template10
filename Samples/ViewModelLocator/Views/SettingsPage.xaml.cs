using Template10.Services.Serialization;
using Template10.Extensions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Template10.Services.Container.Unity.Demo.ViewModels;

namespace Template10.Services.Container.Unity.Demo.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        SettingsPageViewModel ViewModel => DataContext as SettingsPageViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string s && s != null)
            {
                var parameter = s.DeserializeEx().ToString();
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