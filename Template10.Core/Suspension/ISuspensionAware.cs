using System.Threading.Tasks;

namespace Template10.Suspension
{

    public interface ISuspensionAware
    {
        Task OnResumingAsync(ISuspensionState state);
        Task OnSuspendingAsync(ISuspensionState state);
    }

}