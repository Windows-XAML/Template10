using System.Collections.Generic;

namespace Template10.Navigation
{
    public class SessionState : Dictionary<string, object>
    {
        public static SessionState Instance { get; } = new SessionState();

        private SessionState()
        {
            // empty
        }
    }
}
