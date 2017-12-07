using System;

namespace Template10.Common
{
    public class DeferralExArgsEx : EventArgs
    {
        DeferralExManager _manager;
        public DeferralExArgsEx(DeferralExManager manager)
        {
            _manager = manager;
        }

        public DeferralEx GetDeferral() => _manager.GetDeferral();
    }
}
