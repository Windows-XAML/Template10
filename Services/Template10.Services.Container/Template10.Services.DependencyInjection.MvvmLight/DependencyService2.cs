using GalaSoft.MvvmLight.Ioc;

namespace Template10.Services.DependencyInjection
{
    public partial class DependencyService : IDependencyService2<ISimpleIoc>
    {
        ISimpleIoc IDependencyService2<ISimpleIoc>.Container
            => _container;

        IDependencyService2<ISimpleIoc> Two 
            => this as IDependencyService2<ISimpleIoc>;
    }
}
