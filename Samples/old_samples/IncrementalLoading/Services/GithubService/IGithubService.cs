using Template10.Samples.IncrementalLoadingSample.Models;
using System.Threading.Tasks;

namespace Template10.Samples.IncrementalLoadingSample.Services.GithubService
{
    public interface IGithubService
    {
        Task<GithubQueryResult<Repository>> GetRepositoriesAsync(string query, int pageIndex = 1);
    }
}