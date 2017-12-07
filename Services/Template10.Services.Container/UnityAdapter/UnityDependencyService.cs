using System;
using Microsoft.Practices.Unity;

namespace Template10.Services.DependencyInjection
{
    public class UnityDependencyService : DependencyServiceBase, IDependencyService2<IUnityContainer>
    {
        public IUnityContainer Container 
            => (Adapter as UnityContainerAdapter)._container;

        IDependencyService2<IUnityContainer> Two => this as IDependencyService2<IUnityContainer>;

        public UnityDependencyService() :
            base(new UnityContainerAdapter())
        {
            // empty
        }
    }
}
