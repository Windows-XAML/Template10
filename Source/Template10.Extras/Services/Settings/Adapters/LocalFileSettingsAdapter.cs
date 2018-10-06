using System;
using Template10.Extensions;
using Prism.Ioc;
using Template10.Services.File;
using Template10.Services.Serialization;

namespace Template10.Services.Settings
{
    public class LocalFileSettingsAdapter : ISettingsAdapter
    {
        private readonly IFileService _helper;

        public LocalFileSettingsAdapter()
            : this(Prism.PrismApplicationBase.Current.Container.Resolve<ISerializationService>())
        {
            // empty
        }

        public LocalFileSettingsAdapter(ISerializationService serializationService)
        {
            _helper = new File.FileService(serializationService);
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
