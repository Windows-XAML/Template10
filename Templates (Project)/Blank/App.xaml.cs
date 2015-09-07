using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Blank
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Bootstrapper
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService

    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.MainPage));
            return Task.FromResult<object>(null);
        }
    }
}
