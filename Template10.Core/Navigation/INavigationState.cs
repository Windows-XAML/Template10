using System.Threading.Tasks;

namespace Template10.Navigation
{

    public interface INavigationState
    {
        Task LoadAsync(string name = null);
        Task SaveAsync(string name = null);
    }
}