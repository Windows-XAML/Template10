using System;
using Template10.Extensions;
using Template10.Services.FileService;
using Template10.Services.Serialization;

namespace Template10.Services.Settings
{
    public class RoamingFileSettingsAdapter : ISettingsAdapter
    {
        private IFileService _helper;

        public RoamingFileSettingsAdapter(ISerializationService serializationService)
        {
            _helper = new FileService.FileService(serializationService);
        }

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
