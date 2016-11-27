using System;
using System.Runtime.CompilerServices;

namespace Template10.Services.Logging
{
    public class LoggingService : ILoggingService
    {
        public static ILoggingService Instance { get; } = new LoggingService();
        private LoggingService()
        {
            // private constructor
        }

        public delegate void DebugWriteDelegate(string text = null, Severities severity = Severities.Info, Targets target = Targets.Debug, [CallerMemberName]string caller = null);

        public DebugWriteDelegate WriteLine { get; set; } = DefaultWriteLine;

        private static void DefaultWriteLine(string text = null, Severities severity = Severities.Info, Targets target = Targets.Debug, [CallerMemberName]string caller = null)
        {
            var enabled = Template10.Settings.LogginEnabled;
            if (!enabled) return;

            switch (target)
            {
                case Targets.Debug:
                    System.Diagnostics.Debug.WriteLine($"{DateTime.Now.TimeOfDay.ToString()} {severity} {caller} {text}");
                    break;
                case Targets.Log:
                    throw new NotImplementedException();
            }
        }
    }
}
