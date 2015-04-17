using System;

namespace Template10.Mvvm.IoC
{
    public class Container : IContainer
    {
        GalaSoft.MvvmLight.Ioc.SimpleIoc _container;

        public Container()
        {
            _container = new GalaSoft.MvvmLight.Ioc.SimpleIoc();
        }

        public bool IsRegistered<T>()
            where T : class
        {
            return this._container.IsRegistered<T>();
        }

        public void Register<T, C>()
            where T : class
            where C : class, T
        {
            this._container.Register<T, C>();
        }

        public void Register<T>(T instance)
            where T : class
        {
            this._container.Register<T>(() => instance);
        }

        public T Resolve<T>()
        {
            try
            { return _container.GetInstance<T>(); }
            catch (Exception ex)
            { throw new Exception(string.Format("Error resolving {0}", typeof(T)), ex); }
        }
    }
}
