using Windows.Storage;

namespace Template10.Services.Settings
{
    public class RoamingSettingsAdapter : ISettingsAdapter
    {
        private ApplicationDataContainer _container;

        public RoamingSettingsAdapter()
            => _container = ApplicationData.Current.RoamingSettings;

        public string ReadString(string key)
            => _container.Values[key].ToString();

        public void WriteString(string key, string value)
            => _container.Values[key] = value;
    }
}
