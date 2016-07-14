using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Services.SettingsServices;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Sample.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : ViewModelBase
    {
        SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime
            }
            else
            {
                _settings = SettingsService.Instance;
            }
        }

        public bool UseLightThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Light); }
            set { _settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; base.RaisePropertyChanged(); }
        }


        public bool UseCompactOverlay
        {
            get { return _settings.CompactDisplayMode.Equals(SplitViewDisplayMode.CompactOverlay); }
            set
            {
                _settings.CompactDisplayMode = value ? SplitViewDisplayMode.CompactOverlay : SplitViewDisplayMode.CompactInline;
                base.RaisePropertyChanged();
            }
        }
 

        public bool UseBackgroundChecked
        {
            get { return _settings.UseBackgroundChecked; }
            set { _settings.UseBackgroundChecked = value; base.RaisePropertyChanged(); }
        }
    }

    public class AboutPartViewModel : ViewModelBase
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var v = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }

        public Uri RateMe => new Uri("http://aka.ms/template10");
    }
}
