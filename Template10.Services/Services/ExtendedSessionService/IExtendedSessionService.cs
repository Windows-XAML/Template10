using System.Threading.Tasks;

namespace Template10.Services.ExtendedSessionService
{
    public interface IExtendedSessionService
    {
        Task<bool> StartSaveDataAsync();
        Task<bool> StartUnspecifiedAsync();
    }
}