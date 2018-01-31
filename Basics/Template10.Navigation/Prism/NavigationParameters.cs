using System.Collections;
using System.Collections.Generic;

namespace Prism.Navigation
{
    public class NavigationParameters : INavigationParameters, INavigationParametersInternal
    {
        public object this[string key] { get => throw new System.NotImplementedException(); }

        public int Count => throw new System.NotImplementedException();

        public IEnumerable<string> Keys => throw new System.NotImplementedException();

        public void Add(string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public T GetValue<T>(string key)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetValues<T>(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        void INavigationParametersInternal.Add(string key, object value)
        {
            throw new System.NotImplementedException();
        }

        bool INavigationParametersInternal.ContainsKey(string key)
        {
            throw new System.NotImplementedException();
        }

        T INavigationParametersInternal.GetValue<T>(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
