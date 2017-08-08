namespace Template10.Services.Container
{
    public abstract class ContainerService : IContainerService
    {
        static IContainerService _Default;
        public static IContainerService Default
        {
            get => _Default;
            set => _Default = value;
        }

        public ContainerService(IContainerAdapter adapter, bool setAsDefault = true)
        {
            if (setAsDefault && _Default != null)
            {
                throw new System.Exception("Default ContainerService has already been set.");
            }
            _adapter = adapter;
            if (setAsDefault)
            {
                _Default = this;
            }
        }

        public IContainerAdapter _adapter { get; set; }

        public T Resolve<T>() where T : class
            => _adapter.Resolve<T>();

        public void Register<F, T>() where F : class where T : F
            => _adapter.Register<F, T>();

        public void Register<F>(F instance) where F : class
            => _adapter.Register<F>(instance);
    }
}
