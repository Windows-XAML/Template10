using Messaging.Models;
using Messaging.Shared;
using System.Threading.Tasks;

namespace Messaging.Services.GithubService
{
    public interface IGithubService
    {
        Task<GithubQueryResult<Repository>> GetRepositoriesAsync(string query, int pageIndex = 1);
    }
}