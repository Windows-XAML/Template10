using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Mvvm.Container
{
    public abstract class ContainerServiceBase_SimpleIoC : IContainer
    {
        GalaSoft.MvvmLight.Ioc.ISimpleIoc _container;

        public ContainerServiceBase_SimpleIoC()
        {
            this._container = new GalaSoft.MvvmLight.Ioc.SimpleIoc();
        }

        public T Resolve<T>(T type) where T : System.Type
        {
            try
            {
                return _container.GetInstance<T>();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error resolving {0}", type), ex);
            }
        }

        public string Register<T, C>(Lifetimes lifetime = Lifetimes.Singleton)
            where T : System.Type
            where C : T
        {
            Func<T> factory = () =>
            {
                try
                {
                    return Activator.CreateInstance(typeof(C)) as T;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to construct {0}.", typeof(C)), ex);
                }
            };
            return this.Register<T>(factory, Lifetimes.Singleton);
        }

        public string Register<T>(T instance)
            where T : System.Type
        {
            Func<T> factory = () => instance;
            return this.Register<T>(factory, Lifetimes.Singleton);
        }

        public string Register<T>(Func<T> factory, Lifetimes lifetime = Lifetimes.Singleton)
            where T : System.Type
        {
            var key = Guid.NewGuid().ToString();
            switch (lifetime)
            {
                case Lifetimes.Singleton:
                    this._container.Register<T>(factory, key = Guid.Empty.ToString());
                    break;
                case Lifetimes.Everytime:
                    this._container.Register<T>(factory, key);
                    break;
            }
            return key;
        }

        public bool IsRegistered<T>()
            where T : System.Type
        {
            return this._container.IsRegistered<T>();
        }

        public void Unregister<T>()
            where T : System.Type
        {
            if (this.IsRegistered<T>())
                this._container.Unregister<T>();
        }

        public void Unregister<T>(string key)
            where T : System.Type
        {
            if (this.IsRegistered<T>())
                this._container.Unregister<T>(key);
        }

        public void Unregister<T>(T instance)
            where T : System.Type
        {
            if (this.IsRegistered<T>())
                this.Unregister<T>(instance);
        }
    }

}
