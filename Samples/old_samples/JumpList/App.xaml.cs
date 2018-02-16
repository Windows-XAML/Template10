using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Template10.Samples.JumpListSample
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

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            string arguments = string.Empty;
            if (DetermineStartCause(args) == AdditionalKinds.JumpListItem)
            {
                arguments = (args as LaunchActivatedEventArgs).Arguments;
                FileReceived?.Invoke(this, arguments);
            }
            NavigationService.Navigate(typeof(Views.MainPage), arguments);
            return Task.CompletedTask;
        }

        public static event EventHandler<string> FileReceived;
    }
}

