using Windows.Foundation.Collections;

namespace Template10.Common
{
    public partial class DictionaryEx<K, V>
    {
        private class DictionaryExChangedEventArgs : IMapChangedEventArgs<K>
        {
            public DictionaryExChangedEventArgs(CollectionChange change, K key)
            {
                CollectionChange = change;
                Key = key;
            }

            public CollectionChange CollectionChange { get; private set; }
            public K Key { get; private set; }
        }
    }
}
