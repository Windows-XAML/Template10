using System;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;

namespace Prism.Windows
{
    public interface IMyReusableViewModel { }
    public class MyReusableViewModel : IMyReusableViewModel { }

    public class NetCoreContainerExtension : IContainerExtension<IServiceCollection>
    {
        public NetCoreContainerExtension(IServiceCollection serviceCollection)
        {
            Instance = serviceCollection;
        }

        public IServiceCollection Instance { get; }

        private IServiceProvider Provider { get; set; }

        public bool SupportsModules => true;

        public void FinalizeExtension()
        {
            Provider = Instance.BuildServiceProvider();
        }

        public void Register(Type from, Type to)
        {
            Instance.AddTransient(from, to);
        }

        public void Register(Type from, Type to, string name)
        {
            throw new NotSupportedException();
        }

        public void RegisterInstance(Type type, object instance)
        {
            Instance.AddSingleton(type, instance);
        }

        public void RegisterSingleton(Type from, Type to)
        {
            Instance.AddSingleton(from, to);
        }

        public object Resolve(Type type)
        {
            return Provider.GetService(type);
        }

        public object Resolve(Type type, string name)
        {
            throw new NotSupportedException();
        }

        public object ResolveViewModelForView(object view, Type viewModelType)
        {
            throw new NotImplementedException();
        }
    }
}
