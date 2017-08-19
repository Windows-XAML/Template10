namespace Template10.Services.Container
{
    public class MvvmLightContainerService : ContainerService
    {
        public MvvmLightContainerService() :
            base(new MvvmLightContainerAdapter())
        {
            // empty
        }
    }
}
