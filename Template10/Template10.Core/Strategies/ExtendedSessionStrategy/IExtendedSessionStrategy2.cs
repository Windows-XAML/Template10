using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.ExtendedExecution;

namespace Template10.Strategies
{
    public interface IExtendedSessionStrategy2 
    {
        ExtendedExecutionSession ExSession { get; set; }
        Task<bool> StartSavingDataAsync(Action revokedCallback);
        Task<bool> StartUnspecifiedAsync();
    }
}
