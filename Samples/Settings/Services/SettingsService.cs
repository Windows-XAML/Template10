using Template10.Services.SettingsService;

namespace Template10.Samples.SettingsSample.Services
{
    public class SettingsService
    {
        public static readonly SettingsService Instance;
        static SettingsService() { Instance = Instance ?? new SettingsService(); }

        SettingsHelper _helper;
        private SettingsService() { _helper = new SettingsHelper(); }

        public string CustomSetting
        {
            get { return _helper.Read(nameof(CustomSetting), string.Empty); }
            set { _helper.Write(nameof(CustomSetting), value); }
        }
    }
}
