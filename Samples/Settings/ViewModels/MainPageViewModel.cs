using Template10.Mvvm;

namespace Template10.Samples.SettingsSample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        Services.SettingsService _settingsService;
        public MainPageViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                _settingsService = Services.SettingsService.Instance;

                // initial values
                WriteValue = ReadValue = _settingsService.CustomSetting;

                // write value on changes
                PropertyChanged += (s, e) =>
                {
                    _settingsService.CustomSetting = WriteValue;
                    ReadValue = _settingsService.CustomSetting;
                };
            }
        }

        string _WriteValue = default(string);
        public string WriteValue { get { return _WriteValue; } set { Set(ref _WriteValue, value); } }

        string _ReadValue = default(string);
        public string ReadValue { get { return _ReadValue; } set { Set(ref _ReadValue, value); } }

    }
}
