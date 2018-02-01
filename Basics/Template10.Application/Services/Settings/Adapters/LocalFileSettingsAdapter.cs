using System;
using Prism.Windows.Extensions;
using Prism.Ioc;
using Prism.Windows.Services.FileService;
using Prism.Windows.Services.Serialization;

namespace Prism.Windows.Services.Settings
{
    public class LocalFileSettingsAdapter : ISettingsAdapter
    {
        private IFileService _helper;

        public LocalFileSettingsAdapter()
            : this(PrismApplicationBase.Container.Resolve<ISerializationService>())
        {
            // empty
        }

        public LocalFileSettingsAdapter(ISerializationService serializationService)
        {
            _helper = new FileService.FileService(serializationService);
            SerializationService = serializationService;
        }

        public ISerializationService SerializationService { get; }

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
