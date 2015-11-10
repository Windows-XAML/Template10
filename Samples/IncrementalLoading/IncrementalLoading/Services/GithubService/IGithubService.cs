using BottomAppBar.Models;
using BottomAppBar.Shared;
using System.Threading.Tasks;

namespace BottomAppBar.Services.GithubService
{
    public interface IGithubService
    {
        Task<GithubQueryResult<Repository>> GetRepositoriesAsync(string query, int pageIndex = 1);
    }
}