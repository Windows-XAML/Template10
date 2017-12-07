namespace Template10.Services.DependencyInjection
{
    public interface IContainerConsumer
    {
        TInterface Resolve<TInterface>()
            where TInterface : class;
        TInterface Resolve<TInterface>(string key)
            where TInterface : class;
    }
}
