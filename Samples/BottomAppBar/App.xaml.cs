using System;
using Windows.UI.Xaml;
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

        // runs even if restored from state
        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // setup hamburger shell
            var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
            Window.Current.Content = new Views.Shell(nav);
            return Task.FromResult<object>(null);
        }

        // runs only when not restored from state
        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await Task.CompletedTask;
            NavigationService.Navigate(typeof(Views.MainPage));
        }
    }
}

