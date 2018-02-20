using System;

namespace Prism.Ioc
{
    public class DefaultContainerExtension : IContainerExtension
    {
        public bool SupportsModules => false;

        public void FinalizeExtension()
        {
            throw new NotImplementedException();
        }

        public void Register(Type from, Type to)
        {
            throw new NotImplementedException();
        }

        public void Register(Type from, Type to, string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterInstance(Type type, object instance)
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type from, Type to)
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public object Resolve(Type type, string name)
        {
            return Activator.CreateInstance(type);
        }

        public object ResolveViewModelForView(object view, Type viewModelType)
        {
            return Activator.CreateInstance(viewModelType);
        }
    }
}
