using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Template10.Samples.MasterDetailSample.Views;

namespace Template10.Samples.MasterDetailSample
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(MainPage));
			return Task.CompletedTask;
		}
    }
}
