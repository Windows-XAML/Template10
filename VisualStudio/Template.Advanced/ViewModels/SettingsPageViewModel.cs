using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Extensions;
using Template10.Services.Gesture;
using Template10.Services.Logging;
using Template10.Services.Marketplace;
using Windows.UI.Xaml;

namespace Sample.ViewModels
{
    public class SettingsPageViewModel_DesignTime : SettingsPageViewModel
    {
        public SettingsPageViewModel_DesignTime()
            : base(null, null, null) { }
    }

    public class SettingsPageViewModel : ViewModelBase
    {
        ILoggingService _loggingService;
        Services.ISettingsService _settings;
        IMarketplaceService _marketplace;

        public SettingsPageViewModel(
            ILoggingService loggingService,
            Services.ISettingsService settings,
            IMarketplaceService marketplace)
        {
            _settings = settings;
            _marketplace = marketplace;
            _loggingService = loggingService;
        }

        // Settings

        public bool UseShellBackButton
        {
            get => _settings.ShellBackButtonPreference == ShellBackButtonPreferences.AutoShowInShell ? true : false;
            set => Set(() => { _settings.ShellBackButtonPreference = value ? ShellBackButtonPreferences.AutoShowInShell : ShellBackButtonPreferences.NeverShowInShell; });
        }

        public bool UseLightThemeButton
        {
            get => _settings.DefaultTheme.Equals(ElementTheme.Light);
            set => Set(() => { _settings.DefaultTheme = value ? ElementTheme.Light : ElementTheme.Dark; });
        }

        public string BusyText
        {
            get => _settings.BusyText;
            set => Set(() => { _settings.BusyText = value; });
        }

        public void ShowBusy()
        {
            if (this.TryGetPopup<Template10.Popups.BusyPopup>(out var busy))
            {
                busy.Data.Text = BusyText;
                busy.IsShowing = true;
                Template10.Common.TimeoutEx.InvokeAfter(() => busy.IsShowing = false, TimeSpan.FromSeconds(5));
            }
        }

        // About

        public Uri Logo
            => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName
            => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher
            => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var v = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }

        public async void Review()
            => await _marketplace.LaunchAppReviewInStoreAsync();
    }
}
