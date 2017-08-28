using System.Threading.Tasks;
using Template10.Controls;
using Template10.Samples.MasterDetailSample.Views;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Template10.Samples.MasterDetailSample
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = new Views.Shell(service),
            };
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(MainPage));
			return Task.CompletedTask;
		}
    }
}
