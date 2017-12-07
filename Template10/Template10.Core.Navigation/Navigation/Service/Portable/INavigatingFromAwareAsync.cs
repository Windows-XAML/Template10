using System.Threading.Tasks;

namespace Template10.Portable.Navigation
{
    public interface INavigatingFromParameters : INavigationParameters
    {
    }

    public interface INavigatingFromAwareAsync
    {
        Task OnNavigatingFromAsync(INavigatingFromParameters args);
    }
}