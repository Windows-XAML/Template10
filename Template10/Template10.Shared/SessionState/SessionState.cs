using System.Collections.Generic;

namespace Template10.Common
{
    public class SessionState : Dictionary<string, object>
    {
        public static SessionState Current = new SessionState();

        private SessionState()
        {
            // empty
        }
    }
}
