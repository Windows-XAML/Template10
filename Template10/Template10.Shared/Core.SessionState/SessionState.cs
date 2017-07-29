using System;
using System.Collections.Generic;

namespace Template10.Core
{
    public class SessionState : Dictionary<string, object>
    {
        internal static SessionState _instance;
        public static SessionState Instance() => _instance;

        static SessionState()
        {
            _instance = new SessionState();
        }

        private SessionState()
        {
            Add("Created", DateTime.Now);
        }
    }
}
