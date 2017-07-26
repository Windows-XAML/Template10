using System.Threading.Tasks;
using Template10.Portable.Common;
using Template10.Services.NavigationService;

namespace Template10.Common
{
    public interface IBootStrapper : IBootStrapperShared
    {
        INavigationServiceAsync NavigationService { get; }
        BootstrapperStates Status { get; }
        Task OnStartAsync(Template10StartArgs e);
        void HideSplash();
    }
}
