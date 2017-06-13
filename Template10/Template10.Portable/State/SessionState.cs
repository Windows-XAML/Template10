using System;
using System.Collections.Generic;
using System.Text;

namespace Template10.Portable
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
