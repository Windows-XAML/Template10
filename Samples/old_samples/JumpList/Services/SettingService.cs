namespace Template10.Samples.JumpListSample.Services
{
    class SettingService
    {
        Template10.Services.SettingsService.ISettingsHelper _SettingsHelper;

        public SettingService()
        {
            _SettingsHelper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public string Recent
        {
            get { return _SettingsHelper.Read(nameof(Recent), string.Empty); }
            set { _SettingsHelper.Write(nameof(Recent), value); }
        }
    }
}
