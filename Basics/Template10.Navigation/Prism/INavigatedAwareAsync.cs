using System.Threading.Tasks;

namespace Prism.Navigation
{
    public interface INavigatedAwareAsync
{
    Task OnNavigatedFromAsync(INavigationParameters parameters);
    Task OnNavigatedToAsync(INavigationParameters parameters);
}
}
