using System;
using Windows.UI.Xaml;

namespace Sample.ViewModels
{
    public class SettingsPageViewModel : Sample.Mvvm.ViewModelBase
    {
        Services.SettingsServices.SettingsService _settings;

        public SettingsPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime data
                return;
            }
            _settings = Services.SettingsServices.SettingsService.Instance;
        }

        #region Settings

        public bool UseShellBackButton
        {
            get { return _settings.UseShellBackButton; }
            set { _settings.UseShellBackButton = value; base.RaisePropertyChanged(); }
        }

        public bool UseLightThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Light); }
            set { _settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; base.RaisePropertyChanged(); }
        }

        private string _BusyText = "Please wait...";
        public string BusyText { get { return _BusyText; } set { Set(ref _BusyText, value); } }
        public void ShowBusy() { Views.Shell.SetBusyVisibility(Visibility.Visible, _BusyText); }
        public void HideBusy() { Views.Shell.SetBusyVisibility(Visibility.Collapsed); }

        public void ShowLogin() { Views.Shell.SetLoginVisibility(Visibility.Visible); }

        #endregion

        #region About

        public Uri Logo { get { return Windows.ApplicationModel.Package.Current.Logo; } }
        public string DisplayName { get { return Windows.ApplicationModel.Package.Current.DisplayName; } }
        public string Publisher { get { return Windows.ApplicationModel.Package.Current.PublisherDisplayName; } }
        public string Version
        {
            get
            {
                var ver = Windows.ApplicationModel.Package.Current.Id.Version;
                return ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
            }
        }
        public Uri RateMe { get { return new Uri(""); } }

        #endregion  
    }
}
