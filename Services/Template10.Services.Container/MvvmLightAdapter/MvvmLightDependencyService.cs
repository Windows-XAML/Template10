using System;
using GalaSoft.MvvmLight.Ioc;

namespace Template10.Services.DependencyInjection
{
    public class MvvmLightDependencyService : DependencyServiceBase, IDependencyService2<ISimpleIoc>
    {
        ISimpleIoc IDependencyService2<ISimpleIoc>.Container
            => (Adapter as MvvmLightContainerAdapter)._container;

        IDependencyService2<ISimpleIoc> Two => this as IDependencyService2<ISimpleIoc>;

        public MvvmLightDependencyService() :
            base(new MvvmLightContainerAdapter())
        {
            // empty
        }
    }
}
