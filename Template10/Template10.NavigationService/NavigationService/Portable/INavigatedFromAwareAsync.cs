using System;
using System.Threading.Tasks;

namespace Template10.Portable.Navigation
{
    public interface INavigatedFromParameters : INavigationParameters
    {
    }

    public interface INavigatedFromAwareAsync
    {
        Task OnNavigatedFromAsync(INavigatedFromParameters parameters);
    }
}