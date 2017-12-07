using System.Linq;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Common
{
    public partial class PropertyBagEx : IPropertyBagEx, IPropertyBagExSimple
    {
        public async Task ClearAsync() => await PersistenceStrategy.ClearAsync();

        public async Task<string[]> GetKeysAsync() => await PersistenceStrategy.GetKeysAsync();

        public async Task<TryResult<T>> TryGetAsync<T>(string key)
        {
            var keys = await PersistenceStrategy.GetKeysAsync();
            if (!(keys).Contains(key))
            {
                return new TryResult<T>();
            }
            try
            {
                var result = await PersistenceStrategy.LoadAsync(key);
                var value = SerializationStrategy.Deserialize<T>(result);
                return new TryResult<T> { Success = true, Value = value };
            }
            catch
            {
                return new TryResult<T>();
            }
        }

        public async Task<TryResult<string>> TryGetStringAsync(string key)
        {
            var keys = await PersistenceStrategy.GetKeysAsync();
            if (!(keys).Contains(key))
            {
                return new TryResult<string>();
            }
            try
            {
                var value = await PersistenceStrategy.LoadAsync(key) as string;
                return new TryResult<string> { Success = true, Value = value };
            }
            catch
            {
                return new TryResult<string>();
            }
        }

        public async Task<bool> TrySetAsync<T>(string key, T value)
        {
            try
            {
                var result = SerializationStrategy.Serialize(value);
                await PersistenceStrategy.SaveAsync(key, result);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> TrySetStringAsync(string key, string value)
        {
            try
            {
                await PersistenceStrategy.SaveAsync(key, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        void IPropertyBagExSimple.Write(string key, string value)
            => TrySetStringAsync(key, value).RunSynchronously();

        string IPropertyBagExSimple.Read(string key)
            => TryGetStringAsync(key).Result.Value;
    }
}
