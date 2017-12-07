using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Template10;
using Template10.Extensions;
using Template10.Common;
using Sample.ViewModels;
using Template10.Strategies;
using Sample.Views;
using Sample.Services;
using System;
using Template10.Navigation;
using System.Collections.Generic;
using Template10.Services.Marketplace;
using Template10.Messages;
using Template10.Popups;
using Template10.Services.DependencyInjection;
using Template10.Services.Messaging;
using Windows.UI.Xaml.Controls;

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
            SetupListeners(Central.Messenger);
            return base.OnInitializeAsync();
        }

        public override async Task OnStartAsync(IStartArgsEx e, INavigationService navService, ISessionState sessionState)
        {
            await Task.Delay(0);
            if (await navService.NavigateAsync(PageKeys.MainPage))
            {
                HideSplash();
            }
        }

        public override void SetupDependencies(IContainerBuilder container)
        {
            // setup custom services
            container.Register<IViewModelResolutionStrategy, CustomViewModelResolutionStrategy>();
            container.Register<ILocalDialogService, LocalDialogService>();
            container.Register<ISettingsService, Services.SettingsService>();

            // view models
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

        private void SetupListeners(IMessengerService messenger)
        {
            messenger.Subscribe<UnhandledExceptionMessage>(this, m => ShowError(m.EventArgs.Exception));
        }

        private static void ShowError(Exception ex)
        {
            if (PopupsExtensions.TryGetPopup<SplashPopup>(out var busy))
            {
                busy.IsShowing = false;
            }
        }

        // 

        public static void HideSplash()
        {
            if (PopupsExtensions.TryGetPopup<SplashPopup>(out var pop) && pop.IsShowing)
            {
                pop.IsShowing = false;
            }
        }

        public static void ShowBusy(object content)
        {
            if (PopupsExtensions.TryGetPopup<BusyPopup>(out var pop))
            {
                pop.Content.Text = content?.ToString();
                pop.IsShowing = true;
            }
        }

        public static void HideBusy()
        {
            if (PopupsExtensions.TryGetPopup<BusyPopup>(out var pop) && pop.IsShowing)
            {
                pop.Content.Text = string.Empty;
                pop.IsShowing = false;
            }
        }
    }

    public class CustomViewModelResolutionStrategy : IViewModelResolutionStrategy
    {
        static IDependencyService _dependencyService;

        static CustomViewModelResolutionStrategy()
        {
            _dependencyService = Central.DependencyService;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<object> ResolveViewModelAsync(Type type)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return _dependencyService.Resolve<ITemplate10ViewModel>(type.ToString());
        }

        public async Task<object> ResolveViewModelAsync(Page page)
        {
            return await ResolveViewModelAsync(page.GetType());
        }
    }
}