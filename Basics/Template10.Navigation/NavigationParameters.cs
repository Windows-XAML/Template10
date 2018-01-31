using Prism.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace Template10.Navigation
{
    public class NavigationParameters : INavigationParameters, INavigationParametersInternal
    {
        List<KeyValuePair<string, object>> _list = new List<KeyValuePair<string, object>>();
        List<KeyValuePair<string, object>> _internal = new List<KeyValuePair<string, object>>();

        public NavigationParameters()
        {
            // empty
        }

        public NavigationMode NavigationMode { get; internal set; }
        public NavigationService NavigationService { get; internal set; }

        public object this[string key]
            => _list.FirstOrDefault(x => x.Key == key);

        public IEnumerable<string> Keys
            => throw new NotImplementedException();

        public int Count
            => throw new NotImplementedException();

        public void Add(string key, object value)
            => _list.Add((key, value));

        public bool ContainsKey(string key)
            => _list.Any(x => x.Key == key);

        public T GetValue<T>(string key)
            => _list.Where(x => x.Key == key).Select(x => (T)x.Value).FirstOrDefault();

        public bool TryGetValue<T>(string key, out T value)
        {
            try
            {
                value = GetValue<T>(key);
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        public IEnumerable<T> GetValues<T>(string key)
            => _list.Where(x => x.Key == key).Select(x => (T)x.Value);

        public bool TryGetValues<T>(string key, out IEnumerable<T> values)
        {
            try
            {
                values = GetValues<T>(key);
                return true;
            }
            catch (Exception)
            {
                values = null;
                return false;
            }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _list.GetEnumerator();

        // internal

        void INavigationParametersInternal.Add(string key, object value)
        {
            _internal.RemoveAll(x => x.Key == key);
            _internal.Add(KeyValuePair.Create(key, value));
        }

        T INavigationParametersInternal.GetValue<T>(string key)
            => _internal.Where(x => x.Key == key).Select(x => (T)x.Value).FirstOrDefault();

        bool INavigationParametersInternal.TryGetValue<T>(string key, out T value)
        {
            try
            {
                value = GetValue<T>(key);
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        IEnumerable<T> INavigationParametersInternal.GetValues<T>(string key)
            => _internal.Where(x => x.Key == key).Select(x => (T)x.Value);

        bool INavigationParametersInternal.TryGetValues<T>(string key, out IEnumerable<T> values)
        {
            try
            {
                values = (this as INavigationParametersInternal).GetValues<T>(key);
                return true;
            }
            catch (Exception)
            {
                values = null;
                return false;
            }
        }
    }
}
