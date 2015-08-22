using System;

namespace Minimal.ViewModels
{
    public class SettingsPageViewModel : Minimal.Mvvm.ViewModelBase
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
