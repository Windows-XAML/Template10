using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Blank
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            this.SplashFactory = (e) => null;
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // start the user experience
            NavigationService.Navigate(typeof(Views.MainPage), "123");
            return Task.FromResult<object>(null);
        }
    }
}
