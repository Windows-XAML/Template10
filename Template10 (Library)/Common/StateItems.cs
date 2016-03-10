using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Common
{
    public class StateItems : Dictionary<string, object>, IStateItems
    {
        #region Obsolete 

        [Obsolete("Use Add(string, Object) instead.")]
        public KeyValuePair<StateItemKey, object> Add(Type type, string key, object value)
        {
            if (Contains(type, key))
                throw new ArgumentException("Same type+key exists.");
            var stateKey = new StateItemKey(type, key);
            Add(stateKey.ToString(), value);
            var item = new KeyValuePair<StateItemKey, object>(stateKey, value);
            return item;
        }

        [Obsolete("Do not use", true)]
        public void Remove(Type type) // <-- would not be called oftenly, okey to linear search
        {
            var willRemove = Keys.Where(x => x.StartsWith(type?.ToString())).ToList();
            willRemove.ForEach(x => base.Remove(x));
        }

        [Obsolete("Use Remove(string) instead.")]
        public void Remove(Type type, string key)
        {
            base.Remove(new StateItemKey(type, key).ToString()); 
        }

        //[Obsolete("Use Remove(string) instead.")]
        //public void Remove(object value)
        //{
        //    var willRemove = this.Where(x => Object.Equals(x.Value, value)).ToList();
        //    willRemove.ForEach(x => base.Remove(x.Key));
        //}

        [Obsolete("Use ContainsKey(string) instead.")]
        public bool Contains(Type type, string key, object value)
        {
            object tryGetValue;
            if (TryGetValue(new StateItemKey(type, key).ToString(), out tryGetValue))
            {
                return tryGetValue == value;
            }
            return false;

        }

        [Obsolete("Use ContainsKey(string) instead.")]
        public bool Contains(StateItemKey key) => ContainsKey(key.ToString());

        [Obsolete("Use ContainsKey(string) instead.")]
        public bool Contains(Type type, string key) => ContainsKey(new StateItemKey(type, key).ToString());

        [Obsolete("Use ContainsValue(object) instead.")]
        public bool Contains(object value) => ContainsValue(value);

        [Obsolete("Use Get<T>(key) instead.")]
        public T Get<T>(StateItemKey key)
        {
            return Get<T>(key.Key);
        }

        #endregion

        public T Get<T>(string key)
        {
            object tryGetValue;
            if (TryGetValue(new StateItemKey(typeof(T), key).ToString(), out tryGetValue))
            {
                return (T)tryGetValue;
            }
            else if (TryGetValue(key, out tryGetValue))
            {
                return (T)tryGetValue;
            }
            throw new KeyNotFoundException();
        }

        public bool TryGet<T>(string key, out T value)
        {
            object tryGetValue;
            bool success = false;
            if (success = TryGetValue(new StateItemKey(typeof(T), key).ToString(), out tryGetValue))
                value = (T)tryGetValue;
            else if (success = TryGetValue(key, out tryGetValue))
                value = (T)tryGetValue;
            else
                value = default(T);
            return success;
        }
    }
}
