using Prism.Logging;
using System.Diagnostics;

namespace Prism
{
    public class DebugLogger : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
            Debug.WriteLine($"{priority} {category} {message}");
        }
    }
}
