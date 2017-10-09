namespace Template10.Services.Dependency
{
    public interface IContainerConsumer
    {
        TInterface Resolve<TInterface>()
            where TInterface : class;
        TInterface Resolve<TInterface>(string key)
            where TInterface : class;
    }
}
