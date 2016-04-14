using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Template10.Samples.TilesSample
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            switch (DetermineStartCause(args))
            {
                case AdditionalKinds.SecondaryTile:
                    var tileargs = args as LaunchActivatedEventArgs;
                    NavigationService.Navigate(typeof(Views.DetailPage), tileargs.Arguments);
                    break;
                case AdditionalKinds.Toast:
                    var toastargs = args as ToastNotificationActivatedEventArgs;
                    NavigationService.Navigate(typeof(Views.DetailPage), toastargs.Argument);
                    break;
                case AdditionalKinds.Primary:
                case AdditionalKinds.Other:
                    NavigationService.Navigate(typeof(Views.MainPage));
                    break;
            }
			return Task.CompletedTask;
		}
    }
}
