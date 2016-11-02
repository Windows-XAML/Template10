using System.Threading.Tasks;

namespace Template10.Services.Navigation
{
    public interface IConfirmNavigation : INavigationAware
    {
        Task<bool> CanNavigateAsync();
    }
}