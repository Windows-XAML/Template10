using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Samples.PageKeysSample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var p = Template10.Services.SerializationService.SerializationService.Json.Deserialize<int>(e.Parameter?.ToString());
            base.OnNavigatedTo(e);
        }
    }

    public class MainPageViewModel : Template10.Mvvm.ViewModelBase
    {
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            return Task.CompletedTask;
        }
    }
}