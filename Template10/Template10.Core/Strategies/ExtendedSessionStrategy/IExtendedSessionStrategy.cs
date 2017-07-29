using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;

namespace Template10.Strategies
{
    public interface IExtendedSessionStrategy : IDisposable
    {
        Task StartupAsync(IStartArgsEx e);
        Task SuspendingAsync();
    }
}
