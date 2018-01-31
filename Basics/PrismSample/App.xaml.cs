using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PrismSample.Views;
using Template10.Application;
using Template10.Navigation;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace PrismSample
{
    sealed partial class App : BootStrapper
    {
        private ShellPage _shell;
        private INavigationServiceUwp _navigationService;

        public App()
        {
            InitializeComponent();
        }

        public override void Initialize(StartArgs args)
        {
            Window.Current.Content = _shell = new ShellPage(out _navigationService);
            PageNavigationRegistry.Register(nameof(MainPage), typeof(MainPage));
        }

        public override async Task StartAsync(StartArgs args, StartKinds activate)
        {
            return;

            var path = NavigationPathBuilder
                .Create(true, nameof(MainPage), ("Record", "123"))
                .Append(nameof(MainPage), ("Record", "234"))
                .Append(nameof(MainPage), ("Record", "345"))
                .ToString();
            await _navigationService.NavigateAsync(path);
        }
    }
}
