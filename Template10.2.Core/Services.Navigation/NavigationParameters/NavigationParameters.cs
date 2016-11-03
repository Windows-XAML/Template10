using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using Template10.BCL;
using Template10.Utils;
using Windows.Foundation.Collections;
using Template10.Services.Serialization;
using System.Reflection;

namespace Template10.Services.Navigation
{

    public class NavigationParameters : ObservableDictionary<string, object>, INavigationParameters,
        ILogicHost<ISerializationService>
    {
        ISerializationService ILogicHost<ISerializationService>.Instance { get; set; } = Serialization.SerializationService.Instance;

        ISerializationService SerializationService { get; } = Serialization.SerializationService.Instance;

        public NavigationParameters(params KeyValuePair<string, object>[] list)
        {
            foreach (var item in list)
            {
                AddSafely(item.Key, item.Value);
            }
        }

        public NavigationParameters(Uri uri) : this(new[] { new KeyValuePair<string, object>(nameof(Parameter), uri) })
        {
            // nothing
        }

        public NavigationParameters(object value) : this(new[] { new KeyValuePair<string, object>(nameof(Parameter), value) })
        {
            // nothing
        }

        public NavigationParameters(string key, object value) : this(new[] { new KeyValuePair<string, object>(key, value) })
        {
            // nothing
        }

        public NavigationParameters(KeyValuePair<string, object> pair) : this(new[] { pair })
        {
            // nothing
        }

        public T Parameter<T>(string name = nameof(Parameter))
        {
            if (ContainsKey(nameof(Parameter)))
            {
                return (T)this[nameof(Parameter)];
            }
            return default(T);
        }

        #region private methods

        void AddSafely(string key, object value)
        {
            var required = App.Settings.RequireSerializableNavigationParameters;
            if (required && !SerializationService.CanSerialize(value))
            {
                throw new Exception($"{value} / NavigationParameter is not serializable. NavigationParameters must be serializable. See App.Settings.RequireSerializableNavigationParameters for more.");
            }
            if (ContainsKey(key))
            {
                this[key] = value;
            }
            else
            {
                AddSafely(key, value);
            }
            if (value is Uri)
            {
                foreach (var item in (value as Uri).QueryString())
                {
                    AddSafely(item.Name, item.Value);
                }
            }
            else if (value is string && UriUtils.IsUri(value as string))
            {
                var uri = new Uri(value as string);
                foreach (var item in uri.QueryString())
                {
                    AddSafely(item.Name, item.Value);
                }
            }
        }

        #endregion
    }
}