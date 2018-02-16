using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Template10.Samples.MessagingSample
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {
        public static Prism.Events.EventAggregator EventAggregator;

        public App()
        {
            InitializeComponent();
            EventAggregator = new Prism.Events.EventAggregator();
        }

        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            var evt = EventAggregator.GetEvent<Template10.Samples.MessagingSample.Messages.UpdateDateTimeMessage>();
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (s, e) => evt.Publish(DateTime.Now);
            timer.Start();

            return Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.MainPage));
            await Task.Yield();
        }
    }
}

