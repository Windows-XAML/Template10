using System.Threading.Tasks;

namespace Template10.Navigation
{

    public interface INavigationState
    {
        Task<bool> LoadAsync(string name = null);
        Task<bool> SaveAsync(string name = null);
    }
}