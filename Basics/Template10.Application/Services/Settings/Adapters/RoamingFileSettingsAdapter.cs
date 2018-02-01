using System;
using Prism.Windows.Extensions;
using Prism.Ioc;
using Prism.Windows.Services.FileService;
using Prism.Windows.Services.Serialization;

namespace Prism.Windows.Services.Settings
{
    public class RoamingFileSettingsAdapter : ISettingsAdapter
    {
        private IFileService _helper;

        public RoamingFileSettingsAdapter()
          : this(PrismApplicationBase.Container.Resolve<ISerializationService>())
        {
            // empty
        }

        public RoamingFileSettingsAdapter(ISerializationService serializationService)
        {
            _helper = new FileService.FileService(serializationService);
            SerializationService = serializationService;
        }

        public ISerializationService SerializationService { get; }

        public RoamingFileSettingsAdapter(IFileService fileService)
        {
            _helper = fileService;
        }

        public string ReadString(string key)
            => _helper.ReadStringAsync(key, StorageStrategies.Roaming).Result;

        public void WriteString(string key, string value)
        {
            if (!_helper.WriteStringAsync(key, value, StorageStrategies.Roaming).Result)
            {
                throw new Exception();
            }
        }
    }
}
