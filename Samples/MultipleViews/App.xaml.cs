using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Data;

namespace Sample
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    [Bindable]
    sealed partial class App : Template10.Common.BootStrapper
    {

        public App()
        {
            InitializeComponent();
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.MainPage));
            await Task.CompletedTask;
        }
    }
}
