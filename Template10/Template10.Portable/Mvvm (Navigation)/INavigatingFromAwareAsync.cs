using System.Threading.Tasks;

namespace Template10.Mvvm
{
    public interface INavigatingFromAwareAsync
    {
        Task OnNavigatingFromAsync(INavigatingFromParameters args);
    }
}