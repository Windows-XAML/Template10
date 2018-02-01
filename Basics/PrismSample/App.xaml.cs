using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PrismSample.Views;
using Template10.Application;
using Template10.Application.Services;
using Template10.Navigation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace PrismSample
{
    sealed partial class App : BootStrapper
    {
        private ShellPage _shell;

        public App()
        {
            InitializeComponent();
        }

        public override void Initialize(StartArgs args)
        {
            Window.Current.Content = new SplashPage();

            _shell = new ShellPage();
            NavigationRegistry.Register(nameof(MainPage), typeof(MainPage));
        }

        public override async Task StartAsync(StartArgs args, StartKinds activate)
        {
            var path = PathBuilder
                .Create(true, nameof(MainPage), ("Record", "123"))
                .Append(nameof(MainPage), ("Record", "234"))
                .Append(nameof(MainPage), ("Record", "345"))
                .Append(nameof(MainPage), ("Record", "456"))
                .Append(nameof(MainPage), ("Record", "567"))
                .ToString();
            await _shell.ShellView.NavigationService.NavigateAsync(path);
            Window.Current.Content = _shell;
        }
    }
}
