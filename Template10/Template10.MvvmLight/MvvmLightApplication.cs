using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.Messenger;
using Template10.Strategies;

namespace Template10
{
    public abstract class Template10Application
        : BootStrapper
    {
        public Template10Application()
            : base()
        {
            var container = Services.Container.ContainerService.Default = new Services.Container.UnityContainerService();
            container.Register<IMessengerService, MvvmLightMessengerService>();
        }

        public Template10Application(IBootStrapperStrategy strategy = null) : base(strategy)
        {
        }
    }
}
