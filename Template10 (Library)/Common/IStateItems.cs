using System;
using System.Collections.Generic;

namespace Template10.Common
{
    public interface IStateItems
    {
        bool ContainsKey(string key);
        bool TryGet<T>(string key, out T value);
        T Get<T>(string key);
        void Add(string key, object value);
        bool Remove(string key);
        void Clear();

        object this[string key] { get; }

        #region Obsolete 

        [Obsolete("Use Add(string, object) instead.")]
        KeyValuePair<StateItemKey, object> Add(Type type, string key, object value);

        [Obsolete("Use ContainsKey(string) instead.")]
        bool Contains(StateItemKey key);

        [Obsolete("Use ContainsKey(string) instead.")]
        bool Contains(Type type, string key);

        [Obsolete("Use ContainsKey(string) instead.")]
        bool Contains(Type type, string key, object value);

        [Obsolete("Use Get<T>(string) instead.")]
        T Get<T>(StateItemKey key);

        [Obsolete("Use Remove(string) instead.")]
        void Remove(Type type);

        [Obsolete("Use Remove(string) instead.")]
        void Remove(Type type, string key);

        #endregion
    }
}