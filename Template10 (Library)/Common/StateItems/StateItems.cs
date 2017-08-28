using System.Collections.Generic;

namespace Template10.Common
{
    public class StateItems : ObservableDictionary<string, object>, IStateItems
    {
        public T Get<T>(string key)
        {
            if (TryGetValue(key, out object tryGetValue))
            {
                return (T)tryGetValue;
            }
            throw new KeyNotFoundException();
        }

        public bool TryGet<T>(string key, out T value)
        {
            bool success = false;
            if (success = TryGetValue(key, out object tryGetValue))
            {
                value = (T)tryGetValue;
            }
            else
            {
                value = default(T);
            }
            return success;
        }
    }
}
