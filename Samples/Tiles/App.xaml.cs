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

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            switch (DetermineStartCause(args))
            {
                case AdditionalKinds.SecondaryTile:
                    var e = args as LaunchActivatedEventArgs;
                    NavigationService.Navigate(typeof(Views.DetailPage), e.Arguments);
                    break;
                case AdditionalKinds.Primary:
                case AdditionalKinds.Toast:
                case AdditionalKinds.Other:
                    NavigationService.Navigate(typeof(Views.MainPage));
                    break;
            }
            return Task.FromResult<object>(null);
        }
    }
}
