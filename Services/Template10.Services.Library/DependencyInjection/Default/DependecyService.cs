using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.DependencyInjection
{
    public class DependecyService : IDependencyService
    {
        public DependecyService()
        {
            throw new NotImplementedException("Custom implementation required");
        }

        public void RegisterInstance<TClass>(TClass instance) where TClass : class
        {
            throw new NotImplementedException();
        }

        public TInterface Resolve<TInterface>() where TInterface : class
        {
            throw new NotImplementedException();
        }

        public TInterface Resolve<TInterface>(string key) where TInterface : class
        {
            throw new NotImplementedException();
        }

        void IContainerBuilder.Register<TInterface, TClass>(string key)
        {
            throw new NotImplementedException();
        }

        void IContainerBuilder.Register<TInterface, TClass>()
        {
            throw new NotImplementedException();
        }

        void IContainerBuilder.RegisterInstance<TInterface, TClass>(TClass instance)
        {
            throw new NotImplementedException();
        }
    }
}
