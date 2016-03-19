using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace JumpList
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            Template10.Services.LoggingService.LoggingService.Enabled = true;
        }

        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            return base.OnInitializeAsync(args);
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            string arguments = string.Empty;
            if (DetermineStartCause(args) == AdditionalKinds.JumpListItem)
            {
                arguments = (args as LaunchActivatedEventArgs).Arguments;
                FileReceived?.Invoke(this, arguments);
            }
            NavigationService.Navigate(typeof(Views.MainPage), arguments);
        }

        public static event EventHandler<string> FileReceived;
    }
}

