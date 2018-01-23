using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace Template10.Navigation
{
    public class NavigationParameters : INavigationParameters, INavigationParametersInteral
    {
        List<(string Key, object Value)> _list = new List<(string Key, object Value)>();
        List<(string Key, object Value)> _internal = new List<(string Key, object Value)>();

        public NavigationParameters()
        {
            // empty
        }

        public NavigationParameters(string path)
        {
            if (path.Contains("/"))
            {
                throw new Exception("Path contains '/'. This is an invalid format.");
            }
            if (Uri.TryCreate(path, UriKind.Absolute, out var uri))
            {
                var query = new WwwFormUrlDecoder(uri.Query);
                foreach (var item in query)
                {
                    Add(item.Name, item.Value);
                }
            }
            else
            {
                // empty is okay
            }
        }

        public object this[string key]
            => _list.FirstOrDefault(x => x.Key == key);

        public Uri NavigationUri { get; private set; }

        public NavigationMode NavigationMode { get; private set; }

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

        public IEnumerable<T> GetValues<T>(string key)
            => _list.Where(x => x.Key == key).Select(x => (T)x.Value);

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

        // internal

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        void INavigationParametersInteral.AddInternalParameter(string key, object value)
        {
            _internal.RemoveAll(x => x.Key == key);
            _internal.Add((key, value));
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        T INavigationParametersInteral.GetValue<T>(string key)
            => _internal.Where(x => x.Key == key).Select(x => (T)x.Value).FirstOrDefault();

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        IEnumerable<T> INavigationParametersInteral.GetValues<T>(string key)
            => _internal.Where(x => x.Key == key).Select(x => (T)x.Value);

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        bool INavigationParametersInteral.TryGetInternalParameter<T>(string key, out T value)
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
    }
}
