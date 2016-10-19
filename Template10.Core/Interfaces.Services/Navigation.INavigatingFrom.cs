using System.Threading.Tasks;

namespace Template10.Interfaces.Services.Navigation
{
    public interface IConfirmNavigation : INavigationAware
    {
        Task<bool> CanNavigateAsync();
    }
}