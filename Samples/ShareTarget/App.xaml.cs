using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.ShareTargetSample
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
                var key = nameof(ShareOperation);
                if (SessionState.ContainsKey(key))
                    SessionState.Remove(key);
                SessionState.Add(key, shareArgs.ShareOperation);

                var frame = new Frame();
                var nav = NavigationServiceFactory(BackButton.Ignore, ExistingContent.Exclude, frame);
                Window.Current.Content = frame;
                nav.Navigate(typeof(Views.SharePage), key);
            }
            else
            {
                var nav = NavigationService ?? NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                Window.Current.Content = nav.Frame;
                nav.Navigate(typeof(Views.MainPage));
            }
            return Task.CompletedTask;
        }
    }
}

