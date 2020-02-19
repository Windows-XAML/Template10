using System;
using Windows.Storage;

namespace Template10.Services.Settings
{
    public class LocalSettingsAdapter : ISettingsAdapter
    {
        private ApplicationDataContainer _container;

        public LocalSettingsAdapter(ISerializationService serializationService)
        {
            _container = ApplicationData.Current.LocalSettings;
            SerializationService = serializationService;
        }

        public ISerializationService SerializationService { get; }

        public string ReadString(string key)
            => _container.Values[key].ToString();

        public void WriteString(string key, string value)
            => _container.Values[key] = value;
    }
}
