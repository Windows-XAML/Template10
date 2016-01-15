﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.LoggingService
{
    public delegate void DebugWriteDelegate(string text = null, Severities severity = Severities.Info, Targets target = Targets.Debug, [CallerMemberName]string caller = null);

    public enum Severities { Trace, Info, Warning, Error, Critical }

    public enum Targets { Debug, Log }

    public static class LoggingService
    {
        public static bool Enabled { get; set; } = false;

        public static DebugWriteDelegate WriteLine { get; set; } = new DebugWriteDelegate(WriteLineInternal);

        private static void WriteLineInternal(string text = null, Severities severity = Severities.Info, Targets target = Targets.Debug, [CallerMemberName]string caller = null)
        {
            switch (target)
            {
                case Targets.Debug:
                    System.Diagnostics.Debug.WriteLineIf(Enabled, $"{DateTime.Now.TimeOfDay.ToString()} {severity} {caller} {text}");
                    break;
                case Targets.Log:
                    throw new NotImplementedException();
            }
        }
    }
}
