using Prism.Ioc;
using Unity;

namespace Template10
{
    public abstract class ApplicationBase : ApplicationTemplate
    {
        public override IContainerExtension CreateContainerExtension()
        {
            return new UnityContainerExtension(new UnityContainer());
        }
    }
}
