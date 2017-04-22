using System.Threading.Tasks;

namespace Template10.Portable.Navigation
{
    public interface INavigatingToAwareAsync
    {
        Task OnNavigatingToAsync(INavigationParameters args);
    }
}