namespace Template10.Services.Container
{
    public class MvvmLightContainerAdapter : IContainerAdapter
    {
        GalaSoft.MvvmLight.Ioc.ISimpleIoc _container;
        internal MvvmLightContainerAdapter()
        {
            _container = new GalaSoft.MvvmLight.Ioc.SimpleIoc();
        }

        public T Resolve<T>() where T : class
            => _container.GetInstance<T>(typeof(T).ToString()) as T;

        public void Register<F, T>() where F : class where T : class, F
            => _container.Register<F, T>();

        public void Register<F>(F instance) where F : class
            => _container.Register<F>(() => instance);
    }
}
