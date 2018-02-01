using System;
using Prism.Ioc;
using Prism.Windows.Services.Serialization;
using Windows.Storage;

namespace Prism.Windows.Services.Settings
{
    public class LocalSettingsAdapter : ISettingsAdapter
    {
        private ApplicationDataContainer _container;

        public LocalSettingsAdapter()
          : this(PrismApplicationBase.Container.Resolve<ISerializationService>())
        {
            // empty
        }

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
