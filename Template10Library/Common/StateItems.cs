using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Common
{
    public class StateItems : List<StateItem>
    {
        public StateItem Add(Type type, string key, object value)
        {
            if (Contains(type, key))
                throw new ArgumentException("Same type+key exists.");
            var item = new StateItem
            {
                Type = type,
                Key = key,
                Value = value
            };
            this.Add(item);
            return item;
        }

        public void Remove(Type type)
        {
            this.RemoveAll(x => x.Type.Equals(type));
        }

        public void Remove(Type type, string key)
        {
            this.RemoveAll(x => x.Type.Equals(type) && x.Key.Equals(key));
        }

        public void Remove(object value)
        {
            this.RemoveAll(x => x.Value == value);
        }

        public bool Contains(Type type, string key, object value)
        {
            return this.Any(x => (x.Type?.Equals(type) ?? false) && (x.Key?.Equals(key) ?? false) && (x.Value == value));
        }

        public bool Contains(Type type, string key)
        {
            return this.Any(x => (x.Type?.Equals(type) ?? false) && (x.Key?.Equals(key) ?? false));
        }

        public bool Contains(object value)
        {
            return this.Any(x => x.Value == value);
        }

        public new bool Contains(StateItem item)
        {
            return Contains(item.Type, item.Key, item.Value);
        }

        public T Get<T>(string key)
        {
            var item = (T)this.First(x => x.Type == typeof(T) && x.Key == key)?.Value;
            if (item == null)
                throw new KeyNotFoundException();
            return item;
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (!Contains(typeof(T), key))
            {
                value = default(T);
                return false;
            }
            try
            {
                value = (T)this.First(x => x.Type == typeof(T) && x.Key == key).Value;
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
