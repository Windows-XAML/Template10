using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Services.StateService
{
    public class StateItems : ObservableDictionary<string, object>, IStateItems
    {
        private static Services.FileService.FileService FileService { get; } = new Services.FileService.FileService();

        public T Get<T>(string key)
        {
            if (TryGetValue(key, out var tryGetValue))
            {
                return (T)tryGetValue;
            }
            throw new KeyNotFoundException();
        }

        public bool TryGet<T>(string key, out T value)
        {
            var success = false;
            if (success = TryGetValue(key, out var tryGetValue))
            {
                value = (T)tryGetValue;
            }
            else
            {
                value = default(T);
            }
            return success;
        }

        private string Key { get; set; }

        private StateTypes Type { get; set; }

        public static async Task<IStateItems> LoadAsync(string key, StateTypes type)
        {
            return await FileService.ReadFileAsync<IStateItems>(key);
        }

        public async Task SaveAsync()
        {
            await FileService.WriteFileAsync(this, Key);
        }
    }
}
