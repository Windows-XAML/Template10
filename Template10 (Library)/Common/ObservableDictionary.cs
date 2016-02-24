using System.Collections.Generic;
using System.Linq;
using Template10.Utils;
using Windows.Foundation.Collections;

namespace Template10.Common
{
    public class ObservableDictionary<K, V> : IObservableMap<K, V>
    {
        private class ObservableDictionaryChangedEventArgs : IMapChangedEventArgs<K>
        {
            public ObservableDictionaryChangedEventArgs(CollectionChange change, K key)
            {
                CollectionChange = change;
                Key = key;
            }

            public CollectionChange CollectionChange { get; private set; }
            public K Key { get; private set; }
        }

        private Dictionary<K, V> _dictionary = new Dictionary<K, V>();

        public event MapChangedEventHandler<K, V> MapChanged;

        private void RaiseMapChanged(CollectionChange change, K key)
        {
            MapChanged?.Invoke(this, new ObservableDictionaryChangedEventArgs(change, key));
        }

        public void Add(K key, V value)
        {
            _dictionary.Add(key, value);
            RaiseMapChanged(CollectionChange.ItemInserted, key);
        }

        public void Add(KeyValuePair<K, V> item) => Add(item.Key, item.Value);

        public bool Remove(K key)
        {
            if (_dictionary.Remove(key))
            {
                RaiseMapChanged(CollectionChange.ItemRemoved, key);
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            V currentValue;
            if (_dictionary.TryGetValue(item.Key, out currentValue) &&
                Equals(item.Value, currentValue) && _dictionary.Remove(item.Key))
            {
                RaiseMapChanged(CollectionChange.ItemRemoved, item.Key);
                return true;
            }
            return false;
        }

        public V this[K key]
        {
            get { return _dictionary[key]; }
            set
            {
                _dictionary[key] = value;
                RaiseMapChanged(CollectionChange.ItemChanged, key);
            }
        }

        public void Clear()
        {
            var priorKeys = _dictionary.Keys.ToArray();
            _dictionary.Clear();
            priorKeys.ForEach(x => RaiseMapChanged(CollectionChange.ItemRemoved, x));
        }

        public ICollection<K> Keys => _dictionary.Keys;

        public bool ContainsKey(K key) => _dictionary.ContainsKey(key);

        public bool TryGetValue(K key, out V value) => _dictionary.TryGetValue(key, out value);

        public ICollection<V> Values => _dictionary.Values;

        public bool Contains(KeyValuePair<K, V> item) => _dictionary.Contains(item);

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => _dictionary.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            int arraySize = array.Length;
            foreach (var pair in _dictionary)
            {
                if (arrayIndex >= arraySize) break;
                array[arrayIndex++] = pair;
            }
        }
    }
}
