using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer.ShareTarget;

namespace ShareTarget
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {
        public App() { InitializeComponent(); }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            var shareArgs = args as ShareTargetActivatedEventArgs;
            if (shareArgs != null)
            {
                var key = SessionState.Add(typeof(ShareOperation), string.Empty, shareArgs.ShareOperation);
                NavigationService.Navigate(typeof(Views.MainPage), key.Key);
            }
            else
            {
                NavigationService.Navigate(typeof(Views.MainPage));
            }
            return Task.CompletedTask;
        }
    }
}

