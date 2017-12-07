using System;
using Template10.Extensions;
using Template10.Services.FileService;
using Template10.Services.Serialization;

namespace Template10.Services.Settings
{
    public class LocalFileSettingsAdapter : ISettingsAdapter
    {
        private IFileService _helper;

        public LocalFileSettingsAdapter(ISerializationService serializationService)
        {
            _helper = new FileService.FileService(serializationService);
        }

        public LocalFileSettingsAdapter(IFileService fileService)
        {
            _helper = fileService;
        }

        public string ReadString(string key)
            => _helper.ReadStringAsync(key).Result;

        public void WriteString(string key, string value)
        {
            if (!_helper.WriteStringAsync(key, value).Result)
            {
                throw new Exception();
            }
        }
    }
}
