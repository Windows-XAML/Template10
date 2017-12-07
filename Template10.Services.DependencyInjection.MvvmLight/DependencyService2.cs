using GalaSoft.MvvmLight.Ioc;
using Template10.Services.DependencyInjection;

namespace Template10.Impl
{
    public partial class DependencyService : IDependencyService2<ISimpleIoc>
    {
        ISimpleIoc IDependencyService2<ISimpleIoc>.Container
            => _container;

        IDependencyService2<ISimpleIoc> Two 
            => this as IDependencyService2<ISimpleIoc>;
    }
}
