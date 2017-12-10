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
using Windows.UI.Xaml;

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

        public override UIElement CreateRootElement(IStartArgsEx e)
        {
            var shellPage = new ShellPage();
            shellPage.MainNavView.Frame.RegisterNavigationService();
            return shellPage;
        }

        public override Task OnInitializeAsync()
        {
            // setup custom services
            var container = Central.DependencyService;
            container.Register<IViewModelResolutionStrategy, LocalViewModelResolutionStrategy>();
            container.Register<ILocalDialogService, LocalDialogService>();
            container.Register<ISettingsService, SettingsService>();
            container.Register<IProfileRepository, ProfileRepository>();

            // setup pages
            var registry = Template10.Navigation.Settings.PageKeyRegistry;
            registry.Add(PageKeys.MainPage.ToString(), typeof(MainPage));
            container.Register<ITemplate10ViewModel, MainPageViewModel>(typeof(MainPage).ToString());
            registry.Add(PageKeys.DetailPage.ToString(), typeof(DetailPage));
            container.Register<ITemplate10ViewModel, DetailPageViewModel>(typeof(DetailPage).ToString());
            registry.Add(PageKeys.SettingsPage.ToString(), typeof(SettingsPage));
            container.Register<ITemplate10ViewModel, SettingsPageViewModel>(typeof(SettingsPage).ToString());

            // setup settings
            var settings = this.Resolve<ISettingsService>();
            Template10.Settings.ShellBackButtonPreference = settings.ShellBackButtonPreference;
            Template10.Settings.CacheMaxDuration = TimeSpan.FromDays(2);
            Template10.Settings.NavigationBehavior = NavigationBehaviors.Key;
            Template10.Settings.AppResumeBehavior = ResumeBehaviors.Auto;
            Template10.Settings.ParameterBehavior = ParameterBehaviors.Serialize;

            // // setup listener for unhandled exception
            // Central.Messenger.Subscribe<UnhandledExceptionMessage>(this, m =>
            // {
            //     if (PopupsExtensions.TryGetPopup<ErrorPopup>(out var error))
            //     {
            //         error.Data.Error = m.EventArgs.Exception;
            //         error.IsShowing = true;
            //     }
            // });

            UnhandledException += (s, e) =>
            {
                this.Log($"{nameof(UnhandledException)} :: {e.Exception.Message}");
                if (PopupsExtensions.TryGetPopup<ErrorPopup>(out var error))
                {
                    error.Data.Error = e.Exception;
                    error.IsShowing = true;
                }
            };

            // ensure return type
            return base.OnInitializeAsync();
        }

        public override async Task OnStartAsync(IStartArgsEx e, INavigationService navService, ISessionState sessionState)
        {
            await Task.Delay(0);
            if (await navService.NavigateAsync(PageKeys.MainPage.ToString()))
            {
                if (PopupsExtensions.TryGetPopup<SplashPopup>(out var splash) && splash.IsShowing)
                {
                    splash.IsShowing = false;
                }
            }
        }

        #region handy, popup methods

        public static void ShowBusy(object content)
        {
            if (PopupsExtensions.TryGetPopup<BusyPopup>(out var pop))
            {
                pop.Data.Text = content?.ToString();
                pop.IsShowing = true;
            }
        }

        public static void HideBusy()
        {
            if (PopupsExtensions.TryGetPopup<BusyPopup>(out var pop) && pop.IsShowing)
            {
                pop.Data.Text = string.Empty;
                pop.IsShowing = false;
            }
        }

        #endregion  
    }

    public class LocalViewModelResolutionStrategy : IViewModelResolutionStrategy
    {
        public async Task<object> ResolveViewModelAsync(Page page)
            => await ResolveViewModelAsync(page.GetType());

        public async Task<object> ResolveViewModelAsync(Type type)
        {
            await Task.CompletedTask;
            return this.Resolve<ITemplate10ViewModel>(type.ToString());
        }
    }
}