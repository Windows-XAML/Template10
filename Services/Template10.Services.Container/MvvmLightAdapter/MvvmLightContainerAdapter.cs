using System;

namespace Template10.Services.Container
{
    public class MvvmLightContainerAdapter : IContainerAdapter
    {
        GalaSoft.MvvmLight.Ioc.ISimpleIoc _container;
        internal MvvmLightContainerAdapter()
        {
            _container = new GalaSoft.MvvmLight.Ioc.SimpleIoc();
        }

        public TInterface Resolve<TInterface>() where TInterface : class
            => _container.GetInstance<TInterface>(typeof(TInterface).ToString());

        public TInterface Resolve<TInterface>(string key) where TInterface : class
            => throw new NotImplementedException();

        public void Register<TInterface, TClass>()
            where TInterface : class
            where TClass : class, TInterface
            => _container.Register<TInterface, TClass>();

        public void Register<TInterface, TClass>(string key)
            where TInterface : class
            where TClass : class, TInterface
            => throw new NotImplementedException();

        public void Register<F>(F instance) where F : class
            => _container.Register<F>(() => instance);
    }
}
