using System.Threading.Tasks;

namespace Template10.Portable.Navigation
{
    public interface INavigatingToParameters : INavigationParameters
    {
    }

    public interface INavigatingToAwareAsync
    {
        Task OnNavigatingToAsync(INavigationParameters args);
    }
}