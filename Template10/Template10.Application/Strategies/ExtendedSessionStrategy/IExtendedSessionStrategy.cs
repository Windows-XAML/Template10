using System;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Strategies
{
    public interface IExtendedSessionStrategy : IDisposable
    {
        Task StartupAsync(StartupInfo e);
        Task SuspendingAsync();
    }
}
