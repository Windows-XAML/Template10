using Prism.Ioc;
using Unity;

namespace Template10
{
    public abstract class ApplicationBase : ApplicationTemplate
    {
        public override IContainerExtension CreateContainerExtension()
        {
            var container = new UnityContainer();
            return new UnityContainerExtension(container);
        }

        protected override void RegisterInternalTypes(IContainerRegistry containerRegistry)
        {
            // containerRegistry.RegisterTemplate10Services();
            base.RegisterInternalTypes(containerRegistry);
        }
    }
}
