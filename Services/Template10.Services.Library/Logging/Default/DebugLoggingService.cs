using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Template10.Services.Logging
{
    public class DebugLoggingService : ILoggingService
    {
        public bool Enabled { get; set; } = true;

        public void Log(string text, Severities severity = Severities.Info, [CallerMemberName]string caller = null)
        {
            if (Enabled)
            {
                Debug.WriteLine($"{DateTime.Now.TimeOfDay.ToString()} {severity} {caller} {text}", severity);
            }
        }
    }
}
