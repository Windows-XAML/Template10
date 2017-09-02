using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Template10;
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

namespace Sample
{
    public enum PageKeys { MainPage, DetailPage, SettingsPage }

    [Bindable]
    sealed partial class App : BootStrapper<PageKeys>
    {
        public App()
        {
            InitializeComponent();
        }

        //public override void RegisterDependencies(IContainerBuilder container)
        //{
        //    // setup services
        //    container.Register<ISettingsService, SettingsService>();

        //    // setup view-models
        //    container.Register<ITemplate10ViewModel, MainPageViewModel>(typeof(MainPage).ToString());
        //    container.Register<ITemplate10ViewModel, MainPageViewModel>(typeof(MainPage).ToString());
        //    container.Register<ITemplate10ViewModel, DetailPageViewModel>(typeof(DetailPage).ToString());
        //    container.Register<ITemplate10ViewModel, SettingsPageViewModel>(typeof(SettingsPage).ToString());
        //}

        //public override async Task OnInitializeAsync()
        //{
        //    // setup page keys
        //    var page_keys = Template10.Navigation.Settings.PageKeys<PageKeys>();
        //    page_keys.Add(PageKeys.MainPage, typeof(MainPage));
        //    page_keys.Add(PageKeys.DetailPage, typeof(DetailPage));
        //    page_keys.Add(PageKeys.SettingsPage, typeof(SettingsPage));

        //    // setup settings
        //    var settings = Central.Container.Resolve<ISettingsService>();
        //    Template10.Settings.DefaultTheme = settings.DefaultTheme;
        //    Template10.Settings.ShellBackButtonPreference = settings.ShellBackButtonPreference;
        //    Template10.Settings.CacheMaxDuration = settings.CacheMaxDuration;
        //}

        //public override async Task OnStartAsync(IStartArgsEx e, INavigationService navService, ISessionState sessionState)
        //{
        //    await navService.NavigateAsync(typeof(Views.MainPage));
        //}
    }
}
