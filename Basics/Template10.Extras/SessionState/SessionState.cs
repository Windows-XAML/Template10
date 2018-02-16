using System.Collections.Generic;

namespace Template10
{
    public class SessionState : Dictionary<string, object>, ISessionState
    {
        public SessionState()
        {
            // empty
        }
    }
}
