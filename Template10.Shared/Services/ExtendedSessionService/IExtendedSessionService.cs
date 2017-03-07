using System;
using System.Threading.Tasks;

namespace Template10.Services.ExtendedSessionService
{
    public interface IExtendedSessionService : IDisposable
    {
        Task<bool> StartSaveDataAsync();
        Task<bool> StartUnspecifiedAsync();
    }
}