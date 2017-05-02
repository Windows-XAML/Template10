using System.Threading.Tasks;

namespace Template10.Portable.Navigation
{
	public interface INavigatedFromAwareAsync
    {
        Task OnNavigatedFromAsync(INavigationParameters parameters);
    }
}