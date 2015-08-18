using Template10.Mvvm;

namespace Template10.ViewModels
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
    }
}
