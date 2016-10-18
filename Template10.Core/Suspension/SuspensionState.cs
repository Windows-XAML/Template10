using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template10.Suspension
{

    public class SuspensionState : Dictionary<string, object>, ISuspensionState
    {
        Navigation.INavigationService _navigationService;

        public SuspensionState(Navigation.INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public Task LoadAsync(string name = null)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(string name = null)
        {
            throw new NotImplementedException();
        }

        public T TryGet<T>(string key)
        {
            if (ContainsKey(key))
            {
                return default(T);
            }
            try
            {
                return (T)this[key];
            }
            catch
            {
                return default(T);
            }
        }
    }
}