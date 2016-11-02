using System.Threading.Tasks;

namespace Template10.Services.Navigation
{

    public interface ISuspensionAware : INavigationAware
    {
        Task OnResumingAsync(ISuspensionState state);
        Task OnSuspendingAsync(ISuspensionState state);
    }

}