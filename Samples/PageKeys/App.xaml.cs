using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Sample
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public enum Pages { MainPage }

        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            var keys = PageKeys<Pages>();
            keys.Add(Pages.MainPage, typeof(Views.MainPage));
            return base.OnInitializeAsync(args);
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(Pages.MainPage);
			return Task.CompletedTask;
		}
    }
}
