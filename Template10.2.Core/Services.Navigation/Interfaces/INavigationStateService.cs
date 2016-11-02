using System.Threading.Tasks;
using Template10.BCL;

namespace Template10.Services.Navigation
{
    public interface INavigationStateService : IService
    {
        Task<bool> LoadNavigationState(string id, IFrameFacade frame);

        Task<bool> SaveNavigationState(string id, IFrameFacade frame);
    }
}