using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Services.SettingsService
{
    public class SettingsHelper 
    {
        private const int max_setting_length = 8000;
        private StringHelper _helper = new StringHelper();

        public void Write<T>(string key, T value, IPropertySet values, Func<string, ISettingConverter> converter)
        {
            var converted = converter(key).ToStore(value);
            var container = StringToPartsContainer(converted);
            values[key] = container;
        }

        public T Read<T>(string key, T fallback, IPropertySet values, Func<string, ISettingConverter> converter)
        {
            try
            {
                if (!values.ContainsKey(key))
                {
                    throw new KeyNotFoundException(key);
                }
                var container = values[key] as ApplicationDataCompositeValue;
                var value = StringFromPartsContainer(container);
                var converted = converter(key).FromStore<T>(value);
                return converted;
            }
            catch
            {
                return fallback;
            }
        }

        public void Clear(bool deep, IPropertySet values, ApplicationDataContainer container)
        {
            values.Clear();
            if (deep)
            {
                foreach (var item in container.Containers.ToArray())
                {
                    container.DeleteContainer(item.Key);
                }
            }
        }

        public void Remove(string key, IPropertySet values, ApplicationDataContainer container)
        {
            if (values.ContainsKey(key))
            {
                values.Remove(key);
            }
            if (container.Containers.ContainsKey(key))
            {
                container.DeleteContainer(key);
            }
        }

        #region

        private ApplicationDataCompositeValue StringToPartsContainer(string converted)
        {
            var container = new ApplicationDataCompositeValue();
            var parts = _helper.BreakString(converted, max_setting_length);
            foreach (var part in parts.Select((i, x) => new { Index = i, Value = x }))
            {
                container[$"Part{part.Index}"] = part.Value;
            }
            return container;
        }

        private string StringFromPartsContainer(ApplicationDataCompositeValue container)
        {
            var keys = container.Keys.Where(x => x.StartsWith("Part"));
            var parts = container.Where(x => keys.Contains(x.Key)).OrderBy(x => x.Key).Select(x => x.Value);
            return string.Concat(parts.ToArray());
        }

        #endregion  
    }
}
