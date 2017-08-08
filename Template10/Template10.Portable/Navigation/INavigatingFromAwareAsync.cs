using System.Threading.Tasks;

namespace Template10.Navigation
{
    public interface INavigatingFromAwareAsync
    {
        Task OnNavigatingFromAsync(INavigatingFromParameters args);
    }
}