using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace Template10.Samples.FileActivationSample
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {
        public App() { InitializeComponent(); }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            var fileArgs = args as FileActivatedEventArgs;
            if (fileArgs != null && fileArgs.Files.Any())
            {
                var file = fileArgs.Files.First() as StorageFile;
                var content = await FileIO.ReadTextAsync(file);
                NavigationService.Navigate(typeof(Views.MainPage), content);
            }
            else
            {
                NavigationService.Navigate(typeof(Views.MainPage), "Not activated by a file");
            }

        }
    }
}

