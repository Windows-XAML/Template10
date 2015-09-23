using Windows.UI.Xaml;

namespace Sample.ViewModels
{
    public class SettingsPartViewModel : Mvvm.ViewModelBase
    {
        Services.SettingsServices.SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _settings = Services.SettingsServices.SettingsService.Instance;
        }

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
        public string BusyText
        {
            get { return _BusyText; }
            set { Set(ref _BusyText, value); }
        }

        public void ShowBusy()
        {
            Views.Shell.SetBusyVisibility(Visibility.Visible, _BusyText);
        }

        public void HideBusy()
        {
            Views.Shell.SetBusyVisibility(Visibility.Collapsed);
        }
    }
}
