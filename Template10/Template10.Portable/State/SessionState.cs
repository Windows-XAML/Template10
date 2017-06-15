using System;
using System.Collections.Generic;
using System.Text;

namespace Template10
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
