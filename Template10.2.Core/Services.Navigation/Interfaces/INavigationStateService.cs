using System.Threading.Tasks;
using Template10.BCL;

namespace Template10.Services.Navigation
{
    public interface INavigationStateService: IService
    {
        Task<string> LoadFromCacheAsync(string frameId);
        Task<bool> SaveToCacheAsync(string frameId, string state);
    }
}