using System;
using GalaSoft.MvvmLight.Ioc;

namespace Template10.Services.Container
{
    public class MvvmLightContainerService : ContainerService, IContainerService2<ISimpleIoc>
    {
        ISimpleIoc IContainerService2<ISimpleIoc>.Container
            => (Adapter as MvvmLightContainerAdapter)._container;

        IContainerService2<ISimpleIoc> Two => this as IContainerService2<ISimpleIoc>;

        public MvvmLightContainerService() :
            base(new MvvmLightContainerAdapter())
        {
            // empty
        }
    }
}
