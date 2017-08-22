namespace Template10.Services.Container
{
    public interface IContainerAdapter
    {
        TInterface Resolve<TInterface>() where TInterface : class;
        TInterface Resolve<TInterface>(string key) where TInterface : class;
        void Register<TInterface, TClass>() where TInterface : class where TClass : class, TInterface;
        void Register<TInterface, TClass>(string key) where TInterface : class where TClass : class, TInterface;
        void Register<TClass>(TClass instance) where TClass : class;
    }
}
