using System;

namespace Template10.Navigation
{
    public interface INavigationResult
    {
        bool Success { get; }

        Exception Exception { get; }
    }

    public class NavigationResult : INavigationResult
    {
        public bool Success { get; set; }

        public Exception Exception { get; set; }
    }
}
