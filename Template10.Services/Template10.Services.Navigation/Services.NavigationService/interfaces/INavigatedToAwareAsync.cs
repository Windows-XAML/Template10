using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template10.Portable.Navigation
{
    public interface INavigatedToAwareAsync
    {
        Task OnNavigatedToAsync(INavigationParameters parameter);
    }
}