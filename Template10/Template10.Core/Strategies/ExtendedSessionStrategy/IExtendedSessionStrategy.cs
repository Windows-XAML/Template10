using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.StartArgs;

namespace Template10.Strategies
{
    public interface IExtendedSessionStrategy : IDisposable
    {
        Task StartupAsync(ITemplate10StartArgs e);
        Task SuspendingAsync();
    }
}
