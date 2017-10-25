using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Template10.Services.Logging;

namespace Template10.Extensions
{
    public static class LogExtensions
    {
        private static ILoggingService LoggingService
        {
            get
            {
                try
                {
                    return Services.Dependency.DependencyService.Default.Resolve<ILoggingService>();
                }
                catch
                {
                    return null;
                }
            }
        }

        private static void LogPrivate(string text, Severities severity, string caller)
        {
            LoggingService?.Log(text, severity, caller: $"{caller}");
        }

        public static void Log(this object sender, Action action, string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
        {
            LogPrivate(text, severity, caller: $"{caller}");
            action.Invoke();
        }
        public static T Log<T>(this object sender, Func<T> action, string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
        {
            LogPrivate(text, severity, caller: $".{caller}");
            return action.Invoke();
        }
        public static async Task<T> Log<T>(this object sender, Func<Task<T>> action, string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
        {
            LogPrivate(text, severity, caller: $".{caller}");
            return await action.Invoke();
        }
        public static void Log(this object sender, string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
        {
            LogPrivate(text, severity, caller: $"{caller}");
        }
    }
}