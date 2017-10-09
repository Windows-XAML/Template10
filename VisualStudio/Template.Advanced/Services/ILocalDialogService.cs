using System.Threading.Tasks;

namespace Sample.Services
{
    public interface ILocalDialogService
    {
        Task<bool> AreYouSureAsync();
    }
}
