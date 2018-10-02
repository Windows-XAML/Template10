using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace Sample.Services
{
    public interface IJumpListService
    {
        Task<bool> AddAsync(string argument, string display, string image = null, int max = 5, JumpListSystemGroupKind kind = JumpListSystemGroupKind.Recent);
    }
}