using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace Template10.Services.Navigation
{

    public interface ISuspensionAware : INavigationAware
    {
        Task OnResumingAsync(IPropertySet state);
        Task OnSuspendingAsync(IPropertySet state);
    }

}