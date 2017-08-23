using System;
using Microsoft.Practices.Unity;

namespace Template10.Services.Container
{
    public class UnityContainerService : ContainerService, IContainerService2<IUnityContainer>
    {
        public IUnityContainer Container 
            => (Adapter as UnityContainerAdapter)._container;

        IContainerService2<IUnityContainer> Two => this as IContainerService2<IUnityContainer>;

        public UnityContainerService() :
            base(new UnityContainerAdapter())
        {
            // empty
        }
    }
}
