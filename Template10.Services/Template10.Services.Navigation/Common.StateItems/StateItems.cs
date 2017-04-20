using System.Collections.Generic;

namespace Template10.Common
{
    public class StateItems : ObservableDictionary<string, object>, IStateItems
    {
        public T Get<T>(string key)
        {
            object tryGetValue;
            if (TryGetValue(key, out tryGetValue))
            {
                return (T)tryGetValue;
            }
            throw new KeyNotFoundException();
        }

        public bool TryGet<T>(string key, out T value)
        {
            object tryGetValue;
            bool success = false;
            if (success = TryGetValue(key, out tryGetValue))
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
