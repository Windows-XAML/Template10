using Prism.Navigation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Template10.Navigation
{
    public class NavigationParameters : INavigationParameters, INavigationParametersInternal
    {
        Dictionary<string, object> _external = new Dictionary<string, object>();
        Dictionary<string, object> _internal = new Dictionary<string, object>();

        public object this[string key]
            => _external[key];

        public int Count
            => _external.Count;

        public IEnumerable<string> Keys
            => _external.Keys;

        public void Add(string key, object value)
            => _external.Add(key, value);

        public bool ContainsKey(string key)
            => _external.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            => _external.GetEnumerator();

        public T GetValue<T>(string key)
            => (T)_external[key];

        public IEnumerable<T> GetValues<T>(string key)
            => _external.Where(x => x.Key == key).Select(x => (T)x.Value);

        public bool TryGetValue<T>(string key, out T value)
        {
            var success = _external.TryGetValue(key, out var result);
            value = (T)result;
            return success;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => _external.GetEnumerator();

        // internal

        void INavigationParametersInternal.Add(string key, object value)
            => _internal.Add(key, value);

        bool INavigationParametersInternal.ContainsKey(string key)
            => _internal.ContainsKey(key);

        T INavigationParametersInternal.GetValue<T>(string key)
        {
            if (_internal.TryGetValue(key, out var result))
            {
                return (T)result;
            }
            else
            {
                return default(T);
            }
        }
    }
}
