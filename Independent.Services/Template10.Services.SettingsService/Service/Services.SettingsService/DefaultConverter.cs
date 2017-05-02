namespace Template10.Services.SettingsService
{
    using Newtonsoft.Json;

    public class DefaultConverter : ISettingConverter
    {
        StringHelper Helper = new StringHelper();

        public T FromStore<T>(string value)
        {
            try
            {
                value = Helper.DecompressString(value);
                return (T)JsonConvert.DeserializeObject(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public string ToStore<T>(T value)
        {
            try
            {
                var result = JsonConvert.SerializeObject(value, Formatting.None);
                return Helper.CompressString(result);
            }
            catch 
            {
                return string.Empty;
            }
        }
    }
}