namespace Template10.Services.Container
{
    public class ContainerService : IContainerService
    {
        private static IContainerService _instance;
        public static IContainerService GetInstance(IContainerAdapter adapter) => _instance ?? (_instance = new ContainerService(adapter));
        private ContainerService(IContainerAdapter adapter) => _adapter = adapter;

        public IContainerAdapter _adapter { get; set; }

        public T Resolve<T>() where T : class
            => _adapter.Resolve<T>();

        public void Register<F, T>() where F : class where T : F
            => _adapter.Register<F, T>();

        public void Register<F>(F instance) where F : class
            => _adapter.Register<F>(instance);
    }
}
