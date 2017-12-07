using Template10.Common;
using Template10.Services.DependencyInjection;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Common
{
    public static class PropertyBagFactory
    {
         static IDependencyService _container;

        static PropertyBagFactory()
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
                SerializationStrategy = new DefaultPropertyBagSerialziationStrategy(serializer),
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
                SerializationStrategy = new DefaultPropertyBagSerialziationStrategy(serializer),
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
    }
}
