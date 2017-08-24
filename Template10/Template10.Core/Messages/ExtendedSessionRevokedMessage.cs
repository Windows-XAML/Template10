using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.ExtendedExecution;

namespace Template10.Messages
{
    public class ExtendedSessionRevokedMessage
    {
        public ExtendedExecutionReason ExtendedExecutionReason { get; set; }
        public async Task<bool> TryToExtendAsync(Action revokedCallback)
        {
            var s = Central.ContainerService.Resolve<Strategies.IExtendedSessionStrategy2>();
            return await s.StartSavingDataAsync(revokedCallback);
        }
    }
}
