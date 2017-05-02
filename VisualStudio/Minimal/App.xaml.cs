﻿using System.Threading.Tasks;
using Sample.Services.SettingsServices;
using Template10.Common;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Data;

namespace Sample
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        SettingsService _settingsService = SettingsService.Instance;

        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);
            RequestedTheme = _settingsService.AppTheme;
            CacheMaxDuration = _settingsService.CacheMaxDuration;
            ShowShellBackButton = _settingsService.UseShellBackButton;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
