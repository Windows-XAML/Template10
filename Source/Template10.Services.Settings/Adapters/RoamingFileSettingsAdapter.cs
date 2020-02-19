using System;

namespace Template10.Services.Settings
{
    public class RoamingFileSettingsAdapter : ISettingsAdapter
    {
        private readonly IFileService _helper;

        public RoamingFileSettingsAdapter(ISerializationService serializationService)
        {
            _helper = new FileService(serializationService);
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
