using Windows.Storage;

namespace Template10.Services.Settings
{
    public class LocalSettingsAdapter : ISettingsAdapter
    {
        private ApplicationDataContainer _container;

        public LocalSettingsAdapter()
            => _container = ApplicationData.Current.LocalSettings;

        public string ReadString(string key)
            => _container.Values[key].ToString();

        public void WriteString(string key, string value)
            => _container.Values[key] = value;
    }
}
