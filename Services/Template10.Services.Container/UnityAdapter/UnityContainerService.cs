namespace Template10.Services.Container
{
    public class UnityContainerService : ContainerService
    {
        public UnityContainerService() :
            base(new UnityContainerAdapter())
        {
            // empty
        }
    }
}
