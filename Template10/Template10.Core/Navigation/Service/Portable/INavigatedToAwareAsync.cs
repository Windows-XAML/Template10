using System.Threading.Tasks;

namespace Template10.Portable.Navigation
{
    public interface INavigatedToParameters : INavigationParameters
    {
    }

    public interface INavigatedToAwareAsync
    {
        Task OnNavigatedToAsync(INavigatedToParameters parameter);
    }
}