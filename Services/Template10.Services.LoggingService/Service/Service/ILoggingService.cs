using System.Runtime.CompilerServices;

namespace Template10.Services.Logging
{
    public interface ILoggingService
    {
        bool Enabled { get; set; }
        void Log(string text, Severities severity = Severities.Info, [CallerMemberName]string caller = null);
    }
}