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

        public TInterface Resolve<TInterface>() where TInterface : class
            => _adapter.Resolve<TInterface>();

        public TInterface Resolve<TInterface>(string key) where TInterface : class
            => _adapter.Resolve<TInterface>(key);

        public void Register<TInterface, TClass>() where TInterface : class where TClass : class, TInterface
            => _adapter.Register<TInterface, TClass>();

        public void Register<TInterface, TClass>(string key) where TInterface : class where TClass : class, TInterface
            => _adapter.Register<TInterface, TClass>(key);

        public void Register<TClass>(TClass instance) where TClass : class
            => _adapter.Register<TClass>(instance);
    }
}
