using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Template10;
using Template10.Extensions;
using Template10.Core;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Sample.ViewModels;
using Template10.Strategies;
using Template10.Services.Container;
using Sample.Views;
using Sample.Services;
using System;
using Template10.Navigation;
using System.Collections.Generic;
using Template10.Services.Gesture;
using Template10.Services.Marketplace;
using Template10.Services.Dialog;

namespace Sample
{
    public enum PageKeys { MainPage, DetailPage, SettingsPage }

    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override void SetupDependencies(IContainerBuilder container)
        {
            // setup strategies
            container.Register<IViewModelResolutionStrategy, CustomViewModelResolutionStrategy>();

            // setup services
            container.Register<IMarketplaceService, MarketplaceService>();
            container.Register<ISettingsService, SettingsService>();

            // setup view-models
            container.Register<ITemplate10ViewModel, MainPageViewModel>(typeof(MainPage).ToString());
            container.Register<ITemplate10ViewModel, DetailPageViewModel>(typeof(DetailPage).ToString());
            container.Register<ITemplate10ViewModel, SettingsPageViewModel>(typeof(SettingsPage).ToString());
        }

        public override Task OnInitializeAsync()
        {
            SetupPageKeys(this.PageKeys<PageKeys>());
            SetupSettings(this.Resolve<ISettingsService>());
            return base.OnInitializeAsync();
        }

        private void SetupPageKeys(IDictionary<PageKeys, Type> keys)
        {
            keys.Add(PageKeys.MainPage, typeof(MainPage));
            keys.Add(PageKeys.DetailPage, typeof(DetailPage));
            keys.Add(PageKeys.SettingsPage, typeof(SettingsPage));
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
            await navService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
