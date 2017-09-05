using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Template10;
using Template10.Core;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Sample.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Template10.Strategies;
using Template10.Services.Container;
using Sample.Views;
using Template10.Navigation;
using Sample.Services;

namespace Sample
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override Task OnInitializeAsync()
        {
            SetupSettings(new SettingsService());
            return base.OnInitializeAsync();
        }

        private void SetupSettings(ISettingsService settings)
        {
            Template10.Settings.DefaultTheme = settings.DefaultTheme;
            Template10.Settings.ShellBackButtonPreference = settings.ShellBackButtonPreference;
            Template10.Settings.CacheMaxDuration = TimeSpan.FromDays(2);
            Template10.Settings.RequireSerializableParameters = true;
            Template10.Settings.ShowExtendedSplashScreen = true;
        }

        public override async Task OnStartAsync(IStartArgsEx e, INavigationService navService, ISessionState sessionState)
        {
            var args = e.LaunchActivatedEventArgs?.Arguments;
            await navService.NavigateAsync(typeof(MainPage), args);
        }
    }
}
