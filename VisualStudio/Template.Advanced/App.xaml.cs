using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Template10;
using Template10.Extensions;
using Template10.Core;
using Sample.ViewModels;
using Template10.Strategies;
using Template10.Services.Container;
using Sample.Views;
using Sample.Services;
using System;
using Template10.Navigation;
using System.Collections.Generic;
using Template10.Services.Marketplace;
using Template10.Messages;
using Template10.Popups;

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

        public override Task OnInitializeAsync()
        {
            SetupPageKeys(this.PageKeys<PageKeys>());
            SetupSettings(this.Resolve<ISettingsService>());
            Central.Messenger.Subscribe<UnhandledExceptionMessage>(this, OnUnhandledException);
            return base.OnInitializeAsync();
        }

        public override async Task OnStartAsync(IStartArgsEx e, INavigationService navService, ISessionState sessionState)
        {
            await Task.Delay(0);
            if (await navService.NavigateAsync(typeof(MainPage)))
            {
                HideSplash();
            }
        }

        public override void SetupDependencies(IContainerBuilder container)
        {
            container.Register<IViewModelResolutionStrategy, CustomViewModelResolutionStrategy>();
            container.Register<IMarketplaceService, MarketplaceService>();
            container.Register<ISettingsService, SettingsService>();
            container.Register<ITemplate10ViewModel, MainPageViewModel>(typeof(MainPage).ToString());
            container.Register<ITemplate10ViewModel, DetailPageViewModel>(typeof(DetailPage).ToString());
            container.Register<ITemplate10ViewModel, SettingsPageViewModel>(typeof(SettingsPage).ToString());
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
        }

        public void OnUnhandledException(UnhandledExceptionMessage m)
        {
            ShowError(m.EventArgs.Exception);
        }
    }

    sealed partial class App : BootStrapper
    {
        public static void ShowError(Exception ex)
        {
            if (PopupsExtensions.TryGetPopup<SplashPopup>(out var busy))
            {
                busy.IsShowing = false;
            }
        }

        public static void HideSplash()
        {
            if (PopupsExtensions.TryGetPopup<SplashPopup>(out var busy))
            {
                busy.IsShowing = false;
            }
        }

        public static void ShowBusy(object content)
        {
            if (PopupsExtensions.TryGetPopup<BusyPopup>(out var busy))
            {
                busy.Content.Text = content?.ToString();
                busy.IsShowing = true;
            }
        }

        public static void HideBusy()
        {
            if (PopupsExtensions.TryGetPopup<BusyPopup>(out var busy))
            {
                busy.Content.Text = string.Empty;
                busy.IsShowing = false;
            }
        }
    }
}