﻿using System;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Sample.Services.SettingsServices;
using Windows.ApplicationModel.Activation;

namespace Sample
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Bootstrapper
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Cache
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-BackButton
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplashScreen
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SettingsService
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService

    sealed partial class App : Template10.Common.BootStrapper
    {
        ISettingsService _settings;

        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;
        }

        // runs even if restored from state
        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // setup hamburger shell
            var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
            Window.Current.Content = new Views.Shell(nav);
            return Task.FromResult<object>(null);
        }

        // runs only when not restored from state
        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await Task.Delay(5000);
            NavigationService.Navigate(typeof(Views.MainPage));
        }
    }
}
