using Sample.Models;
using System.Threading.Tasks;

namespace Sample.Services
{
    public interface IGithubService
    {
        Task<GithubQueryResult<Repository>> GetRepositoriesAsync(string query, int pageIndex = 1);
    }
}