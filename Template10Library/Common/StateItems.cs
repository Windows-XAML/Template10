using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Common
{
    public class StateItems : List<StateItem>
    {
        public bool Contains(Type type, String key)
        {
            return this.Any(x => (x.Type?.Equals(type) ?? false) && (x.Key?.Equals(key) ?? false));
        }

        public bool Contains(object value)
        {
            return this.Any(x => x.Value == value);
        }

        public T Get<T>(String key)
        {
            return (T)this.First(x => x.Type == typeof(T) && x.Key == key)?.Value;
        }

        public bool TryGet<T>(String key, out T value)
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
