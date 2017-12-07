namespace Template10.Services.DependencyInjection
{
    public interface IContainerBuilder : IContainerConsumer
    {
        void Register<TInterface, TClass>(string key)
           where TInterface : class
           where TClass : class, TInterface;
        void Register<TInterface, TClass>()
             where TInterface : class
             where TClass : class, TInterface;
        void RegisterInstance<TClass>(TClass instance)
            where TClass : class;
        void RegisterInstance<TInterface, TClass>(TClass instance)
            where TInterface : class
            where TClass : class, TInterface;
    }
}
