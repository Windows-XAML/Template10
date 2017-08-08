using System;
using System.Runtime.CompilerServices;

namespace Template10.Services.Logging
{
    public delegate void DebugWriteDelegate(string text = null, Severities severity = Severities.Info, Targets target = Targets.Debug, [CallerMemberName]string caller = null);

    public class LoggingService : ILoggingService
    {
        public LoggingService()
        {
            WriteLine = new DebugWriteDelegate(WriteLineInternal);
        }

        public bool Enabled { get; set; } = true;

        public DebugWriteDelegate WriteLine { get; set; } 

        private void WriteLineInternal(string text = null, Severities severity = Severities.Info, Targets target = Targets.Debug, [CallerMemberName]string caller = null)
        {
            switch (target)
            {
                case Targets.Debug:
                    if (Enabled)
                    {
                        System.Diagnostics.Debug.WriteLine($"{DateTime.Now.TimeOfDay.ToString()} {severity} {caller} {text}");
                    }
                    break;
                case Targets.Log:
                    throw new NotImplementedException();
            }
        }
    }
}
