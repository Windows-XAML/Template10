using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Messaging
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public enum Pages { MainPage, DetailPage, AboutPage }

        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            PageKeys<Pages>().Add(Pages.MainPage, typeof(Views.MainPage));
            PageKeys<Pages>().Add(Pages.DetailPage, null); // TODO
            PageKeys<Pages>().Add(Pages.AboutPage, null); // TODO
            return Task.CompletedTask;
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(Pages.MainPage, 2);
            return Task.CompletedTask;
        }
    }
}
