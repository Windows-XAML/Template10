using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;

namespace Sample
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App() => InitializeComponent();

        public override async Task OnStartAsync(StartKinds startKind, IActivatedEventArgs args)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
