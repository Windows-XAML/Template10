using System.Threading.Tasks;

namespace Template10.Portable.Navigation
{
	public interface INavigatingFromAwareAsync
    {
        Task OnNavigatingFromAsync(INavigationParameters args);
    }
}