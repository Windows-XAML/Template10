using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Template10.Samples.BottomAppBarSample
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        // runs even if restored from state
        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // setup hamburger shell
            var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
            Window.Current.Content = new Views.Shell(nav);
			return Task.CompletedTask;
        }

        // runs only when not restored from state
        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.MainPage));

			return Task.CompletedTask;
        }
    }
}

