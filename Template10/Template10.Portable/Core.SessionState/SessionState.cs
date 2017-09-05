using System;
using System.Collections.Generic;

namespace Template10.Core
{
    public class SessionState : Dictionary<string, object>, ISessionState
    {
        public SessionState()
        {
            // empty
        }
    }
}
