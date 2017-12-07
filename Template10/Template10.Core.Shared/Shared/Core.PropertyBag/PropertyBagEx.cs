using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.DependencyInjection;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Core
{
    // the interface for this is in portable

    public partial class PropertyBagEx : IPropertyBagEx2
    {
        public IPropertyBagExPersistenceStrategy PersistenceStrategy { get; set; }

        public IPropertyBagExSerializationStrategy SerializationStrategy { get; set; }
    }

    public partial class PropertyBagEx : IPropertyBagEx, IPropertyBagExSimple
    {
        private static IDependencyService _container;

        static PropertyBagEx()
        {
            _container = Central.DependencyService;
        }

        public static IPropertyBagEx Create(StorageFolder folder, Services.Serialization.ISerializationService serializer = null)
        {
            if (serializer == null)
            {
                serializer = _container.Resolve<Services.Serialization.ISerializationService>();
            }
            return new PropertyBagEx
            {
                PersistenceStrategy = new FolderPropertyBagPersistenceStrategy(folder),
                SerializationStrategy = new PropertyBagSerialziationStrategy(serializer),
            };
        }

        public static IPropertyBagEx Create(IPropertySet settings, Services.Serialization.ISerializationService serializer = null)
        {
            if (serializer == null)
            {
                serializer = _container.Resolve<Services.Serialization.ISerializationService>();
            }
            return new PropertyBagEx
            {
                PersistenceStrategy = new SettingsPropertyBagPersistenceStrategy(settings),
                SerializationStrategy = new PropertyBagSerialziationStrategy(serializer),
            };
        }

        public static IPropertyBagEx Create()
        {
            return new PropertyBagEx
            {
                PersistenceStrategy = new MemoryPropertyBagPersistenceStrategy(),
                SerializationStrategy = new EmptyPropertyBagSerializationStrategy(),
            };
        }

        protected PropertyBagEx()
        {
            // empty
        }

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
