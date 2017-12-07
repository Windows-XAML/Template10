using System;
using System.Collections.Generic;

namespace Template10.Common
{
    public class SessionState : Dictionary<string, object>, ISessionState
    {
        public SessionState()
        {
            // empty
        }
    }
}
