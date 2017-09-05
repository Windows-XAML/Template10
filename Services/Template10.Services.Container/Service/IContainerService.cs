using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.Container
{
    public interface IContainerConsumer
    {
        TInterface Resolve<TInterface>()
            where TInterface : class;
        TInterface Resolve<TInterface>(string key)
            where TInterface : class;
    }
    public interface IContainerBuilder : IContainerConsumer
    {
        void Register<TInterface, TClass>(string key)
           where TInterface : class
           where TClass : class, TInterface;
        void Register<TInterface, TClass>()
             where TInterface : class
             where TClass : class, TInterface;
        void RegisterInstance<TClass>(TClass instance)
            where TClass : class;
        void RegisterInstance<TInterface, TClass>(TClass instance)
            where TInterface : class
            where TClass : class, TInterface;
    }
    public interface IContainerService : IContainerConsumer, IContainerBuilder
    {
        // empty
    }
}
