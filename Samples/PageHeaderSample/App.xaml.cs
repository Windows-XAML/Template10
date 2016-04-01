using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Template10.Samples.PageHeaderSample
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
	{
		public App()
		{
			InitializeComponent();
		}

		// runs only when not restored from state
		public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
		{
			// navigate to first page
			NavigationService.Navigate(typeof(Views.MainPage));
			return Task.CompletedTask;
		}
	}
}

