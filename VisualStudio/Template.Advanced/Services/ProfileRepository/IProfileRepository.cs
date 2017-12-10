using System.Threading.Tasks;
using Sample.Models;

namespace Sample.Services
{
    public interface IProfileRepository
    {
        Task<Profile> LoadAsync(string key);
        Task SaveAsync(Profile profile);
    }
}