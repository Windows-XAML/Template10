using Prism.Windows;
using Prism.Ioc;
using Unity;
using Prism.Windows.Services.Serialization;
using System;

namespace Prism.Windows
{
    public abstract class PrismApplication : PrismApplicationBase
    {
        protected static IContainerProvider _container;

        public sealed override IContainerExtension CreateContainer()
        {
            var container = new UnityContainerExtension(new UnityContainer());
            _container = container;
            return container;
        }

        public override void RegisterRequiredTypes(IContainerRegistry container)
        {
            base.RegisterRequiredTypes(container);
            container.Register<ISerializationService, NewtonsoftSerializationService>();
        }

        protected static T Resolve<T>() => _container.Resolve<T>();
        protected static T Resolve<T>(string key) => _container.Resolve<T>(key);
        protected static object Resolve(Type type) => _container.Resolve(type);
        protected static object Resolve(Type type, string key) => _container.Resolve(type, key);
    }
}
