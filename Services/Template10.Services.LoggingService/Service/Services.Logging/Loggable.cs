using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Template10.Services.Logging
{
    public abstract class Loggable : ILoggable
    {
        ILoggingService ILoggable.LoggingService { get; set; }
        protected void LogThis(System.Action action, string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
        {
            (this as ILoggable).LogThis(text, severity, caller: $"{GetType()}.{caller}");
            action.Invoke();
        }
        protected T LogThis<T>(System.Func<T> action, string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
        {
            (this as ILoggable).LogThis(text, severity, caller: $"{GetType()}.{caller}");
            return action.Invoke();
        }
        protected async Task<T> LogThis<T>(Func<Task<T>> action, string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
        {
            (this as ILoggable).LogThis(text, severity, caller: $"{GetType()}.{caller}");
            return await action.Invoke();
        }
        protected void LogThis(string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
            => (this as ILoggable).LogThis(text, severity, caller: $"{GetType()}.{caller}");
        void ILoggable.LogThis(string text, Severities severity, string caller)
            => (this as ILoggable).LoggingService.WriteLine(text, severity, caller: $"{GetType()}.{caller}");
    }
}
