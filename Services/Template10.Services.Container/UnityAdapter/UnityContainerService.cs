using System;
using Microsoft.Practices.Unity;

namespace Template10.Services.Container
{
    public interface IUnityContainerService
    {
        IUnityContainer Container { get; }
    }

    public class UnityContainerService : ContainerService, IUnityContainerService
    {
        public IUnityContainer Container 
            => (Adapter as UnityContainerAdapter)._container;

        public UnityContainerService() :
            base(new UnityContainerAdapter())
        {
            // empty
        }
    }
}
