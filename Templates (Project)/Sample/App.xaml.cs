using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;

namespace Template10
{
    sealed partial class App : Common.BootStrapper
    {
        public App() : base()
        {
            this.InitializeComponent();
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            if (startKind == StartKind.Launch)
            {
                this.NavigationService.Navigate(typeof(Views.MainPage));
            }
            else
            {
                // this.NavigationService.Navigate(typeof(Views.SecondPage));
            }
            return Task.FromResult<object>(null);
        }
    }
}
