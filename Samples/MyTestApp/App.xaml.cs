using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace MyTestApp {
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	sealed partial class App : Template10.Common.BootStrapper {
		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App() {
			this.InitializeComponent();
		}

		public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args) {
			NavigationService.Navigate(typeof(Views.MainPage), null);
			return Task.FromResult<object>(null);
		}
	}
}
