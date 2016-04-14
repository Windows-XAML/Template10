using Template10.Samples.IncrementalLoadingSample.Views;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;

namespace Template10.Samples.IncrementalLoadingSample
{
    sealed partial class App : BootStrapper
    {
        public App()
        {
            this.InitializeComponent();
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(MainView));
			return Task.CompletedTask;
		}
    }
}