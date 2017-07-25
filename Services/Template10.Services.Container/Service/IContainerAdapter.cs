namespace Template10.Services.Container
{
    public interface IContainerAdapter
    {
        T Resolve<T>() where T : class;
        void Register<F, T>() where F : class where T : F;
        void Register<F>(F instance) where F : class;
    }
}
