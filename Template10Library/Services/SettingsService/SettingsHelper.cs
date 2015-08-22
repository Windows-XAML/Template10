using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Services.SettingsService
{
    // https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SettingsService

	[Obsolete("Use Settings.Local or Settings.Roaming instead")]
    public class SettingsHelper
    {
        const string parse = "Parse";

        public IPropertySet Container(SettingsStrategy strategy)
        {
            return (strategy == SettingsStrategy.Local)
                ? ApplicationData.Current.LocalSettings.Values
                : ApplicationData.Current.RoamingSettings.Values;
        }

        public bool Exists(string key, SettingsStrategy strategy = SettingsStrategy.Local)
        {
            var settings = Container(strategy);
            return settings.ContainsKey(key);
        }

        public void Remove(string key, SettingsStrategy strategy = SettingsStrategy.Local)
        {
            var settings = Container(strategy);
            if (settings.ContainsKey(key))
                settings.Remove(key);
        }

        public void Write<T>(string key, T value, SettingsStrategy strategy = SettingsStrategy.Local)
        {
            var settings = Container(strategy);
            if (TypeUtil.IsPrimitive(typeof(T)))
            {
                try { settings[key] = value.ToString(); }
                catch { settings[key] = string.Empty; }
            }
            else
            {
                var json = Serialize(value);
                try { settings[key] = json; }
                catch { settings[key] = string.Empty; }
            }
        }

        public T Read<T>(string key, T otherwise, SettingsStrategy strategy = SettingsStrategy.Local)
        {
            var settings = Container(strategy);
            if (!settings.ContainsKey(key))
                return otherwise;
            try
            {
                object untyped;
                if (settings.TryGetValue(key, out untyped))
                {
                    if (untyped == null)
                        return otherwise;
                }
                else
                {
                    return otherwise;
                }
                // attempt to deserialize
                if (TypeUtil.IsPrimitive(typeof(T)))
                {
                    var parse = typeof(T).GetRuntimeMethod(SettingsHelper.parse, new[] { typeof(string) });
                    return (T)parse.Invoke(null, new[] { untyped });
                }
                else
                {
                    var json = untyped.ToString();
                    return Deserialize<T>(json);
                }
            }
            catch { return otherwise; }
        }

        private string Serialize<T>(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        private T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }



        public enum SettingsStrategy { Local, Roam, Temp }
    }

}
