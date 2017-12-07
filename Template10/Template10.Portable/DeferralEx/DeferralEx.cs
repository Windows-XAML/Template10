using System;

namespace Template10.Common
{
    public sealed class DeferralEx
    {
        private Action _callback;
        public DeferralEx(Action callback)
        {
            _callback = callback;
        }
        public void Complete()
        {
            _callback.Invoke();
        }
    }
}
