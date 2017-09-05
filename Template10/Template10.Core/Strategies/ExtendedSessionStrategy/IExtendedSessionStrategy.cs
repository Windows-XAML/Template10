using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;
using Windows.ApplicationModel.ExtendedExecution;

namespace Template10.Strategies
{
    public interface IExtendedSessionStrategy : IDisposable
    {
        bool IsStarted { get; }
        bool IsRevoked { get; }
        bool IsActive { get; }
        ExtendedExecutionReason ExReason { get; }
        Task StartupAsync(IStartArgsEx e);
        Task SuspendingAsync();
    }
}
