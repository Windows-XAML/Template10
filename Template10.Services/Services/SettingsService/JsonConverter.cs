using System;

namespace Template10.Services.SettingsService
{
    using Newtonsoft.Json;

    public class JsonConverter : IStoreConverter
    {
        public object FromStore(string value, Type type) => JsonConvert.DeserializeObject(value, type);
        public string ToStore(object value, Type type) => JsonConvert.SerializeObject(value, Formatting.None);
    }
}