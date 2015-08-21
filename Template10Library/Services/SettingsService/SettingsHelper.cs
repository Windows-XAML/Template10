using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Services.SettingsService
{
    // https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SettingsService
    public class SettingsHelper
    {
        const string parse = "Parse";
        readonly Type[] primitives = new Type[]
        {
            typeof (Enum),
            typeof (String),
            typeof (Char),
            typeof (Guid),

            typeof (Boolean),
            typeof (Byte),
            typeof (Int16),
            typeof (Int32),
            typeof (Int64),
            typeof (Single),
            typeof (Double),
            typeof (Decimal),

            typeof (SByte),
            typeof (UInt16),
            typeof (UInt32),
            typeof (UInt64),

            typeof (DateTime),
            typeof (DateTimeOffset),
            typeof (TimeSpan),
        };

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
            if (IsPrimitive(typeof(T)))
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
                if (IsPrimitive(typeof(T)))
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

        private bool IsPrimitive(Type type)
        {
            var nulls = from t in primitives
                        where t.GetTypeInfo().IsValueType
                        select typeof(Nullable<>).MakeGenericType(t);
            var all = primitives.Concat(nulls);
            if (all.Any(x => x.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo())))
                return true;
            var nullable = Nullable.GetUnderlyingType(type);
            return nullable != null && nullable.GetTypeInfo().IsEnum;
        }

        public enum SettingsStrategy { Local, Roam, Temp }
    }

}
