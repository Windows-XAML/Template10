using System.Threading.Tasks;

namespace Template10.Navigation
{
    public interface IConfirmNavigation : INavigationAware
    {
        Task<bool> CanNavigateAsync();
    }
}