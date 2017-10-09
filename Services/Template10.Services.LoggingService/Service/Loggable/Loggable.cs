using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Template10.Services.Logging
{
    public abstract class Loggable : ILoggable
    {
        ILoggingService ILoggable.LoggingService => Dependency.DependencyService.Default.Resolve<ILoggingService>();
        protected void Log(System.Action action, string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
        {
            (this as ILoggable).Log(text, severity, caller: $"{caller}");
            action.Invoke();
        }
        protected T Log<T>(System.Func<T> action, string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
        {
            (this as ILoggable).Log(text, severity, caller: $".{caller}");
            return action.Invoke();
        }
        protected async Task<T> Log<T>(Func<Task<T>> action, string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
        {
            (this as ILoggable).Log(text, severity, caller: $".{caller}");
            return await action.Invoke();
        }
        protected void Log(string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null)
            => (this as ILoggable).Log(text, severity, caller: $"{caller}");
        void ILoggable.Log(string text, Severities severity, string caller)
            => (this as ILoggable).LoggingService.Log(text, severity, caller: $"{GetType()}.{caller}");
    }
}
