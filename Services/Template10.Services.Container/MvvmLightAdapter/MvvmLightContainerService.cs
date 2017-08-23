using System;
using GalaSoft.MvvmLight.Ioc;

namespace Template10.Services.Container
{
    public interface IMvvmLightContainerService
    {
        ISimpleIoc Container { get; }
    }

    public class MvvmLightContainerService : ContainerService, IMvvmLightContainerService
    {
        ISimpleIoc IMvvmLightContainerService.Container
            => (Adapter as MvvmLightContainerAdapter)._container;

        public MvvmLightContainerService() :
            base(new MvvmLightContainerAdapter())
        {
            // empty
        }
    }
}
