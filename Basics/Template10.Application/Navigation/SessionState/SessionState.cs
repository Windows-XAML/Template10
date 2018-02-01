using System.Collections.Generic;

namespace Prism.Windows.Navigation
{
    public class SessionState : Dictionary<string, object>, ISessionState
    {
        public SessionState()
        {
            // empty
        }
    }
}
