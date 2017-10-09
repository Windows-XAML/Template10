using System.Runtime.CompilerServices;

namespace Template10.Services.Logging
{
    public interface ILoggable
    {
        ILoggingService LoggingService { get; }
        void Log(string text, Severities severity, [CallerMemberName]string caller = null);
    }
}
