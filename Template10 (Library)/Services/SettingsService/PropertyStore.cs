using System;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Services.SettingsService
{
    public class PropertyStore
    {
        protected IPropertyMapping Converters { get; }

        protected IPropertySet Values { get; }

        public PropertyStore(IPropertySet values, IPropertyMapping converters)
        {
            if (values == null || converters == null)
                throw new ArgumentNullException();
            Values = values;
            Converters = converters;
        }

        public bool Exists(string key) => Values.ContainsKey(key);

        public void Remove(string key)
        {
            if (Values.ContainsKey(key))
                Values.Remove(key);
        }

        public void Write<T>(string key, T value)
        {
            var type = typeof(T);
            var converter = Converters.GetConverter(type);

            // Why use a composite value container? 
            // Because it incrases the setting limit to 64k.
            var container = new ApplicationDataCompositeValue();
            container["Value"] = converter.ToStore(value, type);
            Values[key] = container;
        }

        public T Read<T>(string key, T fallback)
        {
            try
            {
                if (!Values.ContainsKey(key))
                    return fallback;
                var converter = Converters.GetConverter(typeof(T));
                var container = converter.FromStore(Values[key].ToString(), typeof(T)) as ApplicationDataCompositeValue;
                return (T)container["Value"];
            }
            catch
            {
                return fallback;
            }
        }
    }
}
