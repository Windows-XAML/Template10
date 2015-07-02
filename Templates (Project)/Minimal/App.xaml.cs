using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;

namespace Template10
{
    sealed partial class App : Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            this.ShowShellBackButton = true;
        }

        public override Task OnInitializeAsync()
        {
            // runs before everything
            return base.OnInitializeAsync();
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // start the user experience
            NavigationService.Navigate(typeof(Views.MainPage));
            return Task.FromResult<object>(null);
        }
    }
}
