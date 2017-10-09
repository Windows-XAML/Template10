using System;
using System.Runtime.CompilerServices;

namespace Template10.Services.Logging
{
    public class LoggingService : ILoggingService
    {
        public static ILoggingService Default => new LoggingService(new DefaultAdapter());

        private ILoggingAdapter _adapter;
        public LoggingService(ILoggingAdapter adapter)
        {
            _adapter = adapter;
        }

        public bool Enabled { get; set; } = true;

        public void Log(string text, Severities severity = Severities.Info, [CallerMemberName]string caller = null)
        {
            if (Enabled)
            {
                _adapter.Log($"{DateTime.Now.TimeOfDay.ToString()} {severity} {caller} {text}", severity);
            }
        }
    }
}
