using System;
using Prism.Windows.Services.Serialization;
using Prism.Ioc;
using Windows.Storage;

namespace Prism.Windows.Services.Settings
{
    public class RoamingSettingsAdapter : ISettingsAdapter
    {
        private ApplicationDataContainer _container;

        public RoamingSettingsAdapter()
           : this(PrismApplicationBase.Container.Resolve<ISerializationService>())
        {
            // empty
        }

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
