using System;
using Windows.Storage;

namespace Template10.Services.Settings
{
    public class RoamingSettingsAdapter : ISettingsAdapter
    {
        private ApplicationDataContainer _container;

        public RoamingSettingsAdapter(ISerializationService serializationService)
        {
            _container = ApplicationData.Current.RoamingSettings;
            SerializationService = serializationService;
        }

        public ISerializationService SerializationService { get; }

        public string ReadString(string key)
                => _container.Values[key].ToString();

        public void WriteString(string key, string value)
            => _container.Values[key] = value;
    }
}
