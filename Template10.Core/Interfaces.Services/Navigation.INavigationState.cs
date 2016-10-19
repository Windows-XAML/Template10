using System.Threading.Tasks;

namespace Template10.Interfaces.Services.Navigation
{

    public interface INavigationState
    {
        Task<bool> LoadAsync(string name = null);
        Task<bool> SaveAsync(string name = null);
    }
}