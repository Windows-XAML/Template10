namespace Template10.NuGet.FileActivationSample
{
    public static class UnityContainerExtension 
    {
        public static T Resolve<T>(this Prism.Ioc.IContainerProvider container)
        {
            return (T)container.Resolve(typeof(T));
        }
    }
}
