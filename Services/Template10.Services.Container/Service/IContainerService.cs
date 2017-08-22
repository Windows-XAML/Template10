using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.Container
{
    public interface IContainerService
    {
        T Resolve<T>() 
            where T : class;
        TInterface Resolve<TInterface, TClass>(string key)
            where TInterface : class
            where TClass : class, TInterface;
        void Register<TInterface, TClass>(string key)
           where TInterface : class
           where TClass : class, TInterface;
       void Register<F, T>() 
            where F : class 
            where T : class, F;
        void RegisterInstance<TClass>(TClass instance) 
            where TClass : class;
        void RegisterInstance<TInterface, TClass>(TClass instance)
            where TInterface : class
            where TClass : class, TInterface;
    }
}
