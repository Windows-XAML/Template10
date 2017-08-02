using System.Collections.Generic;

namespace Template10.Common
{
    public class SessionState : Dictionary<string, object>
    {
        internal static SessionState Instance = new SessionState();

        private SessionState()
        {
            // empty
        }
    }
}
