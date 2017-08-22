namespace Template10.Services.Container
{
    public interface IContainerAdapter
    {
        T Resolve<T>()
        where T : class;
        void Register<TInterface, TClass>(string key)
           where TInterface : class
           where TClass : class, TInterface;
        TInterface Resolve<TInterface, TClass>(string key)
            where TInterface : class
            where TClass : class, TInterface;
        void Register<F, T>()
             where F : class
             where T : class, F;
        void RegisterInstance<TClass>(TClass instance)
            where TClass : class;
        void RegisterInstance<TInterface, TClass>(TClass instance)
            where TInterface : class
            where TClass : class, TInterface;
    }
}
