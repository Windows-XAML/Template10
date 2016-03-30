using IncrementalLoading.Models;
using IncrementalLoading.Shared;
using System.Threading.Tasks;

namespace IncrementalLoading.Services.GithubService
{
    public interface IGithubService
    {
        Task<GithubQueryResult<Repository>> GetRepositoriesAsync(string query, int pageIndex = 1);
    }
}