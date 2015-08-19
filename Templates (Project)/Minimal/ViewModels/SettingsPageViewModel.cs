using System;
using Template10.Mvvm;

namespace Minimal.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        Services.SettingsServices.SettingsService _settings;

        public SettingsPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;
            _settings = new Services.SettingsServices.SettingsService();
        }

        public bool Something
        {
            get { return _settings.Something; }
            set { _settings.Something = value; base.RaisePropertyChanged(); }
        }

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
    }
}
