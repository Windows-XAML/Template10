using System.Threading.Tasks;

namespace Template10.Services.NavigationService
{
    public interface INavigatingAwareAsync
    {
        Task OnNavigatingFromAsync(NavigatingEventArgs args);
    }
}