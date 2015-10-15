using Sample.Models;
using Sample.Shared;
using System.Threading.Tasks;

namespace Sample.Services.GithubService
{
    public interface IGithubService
    {
        Task<GithubQueryResult<Repository>> GetRepositoriesAsync(string query, int pageIndex = 1);
    }
}