using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;

namespace Template10.Services.Profile
{
    public interface IUserService
    {
        Task<IUserEx> GetUserExAsync();
    }
}
