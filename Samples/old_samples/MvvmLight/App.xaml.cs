using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Template10.Samples.MvvmLightSample
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }
        
        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.MainPage), "Runtime value");
			return Task.CompletedTask;
		}
    }
}
