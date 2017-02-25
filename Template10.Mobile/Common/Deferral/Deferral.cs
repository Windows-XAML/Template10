using System;

namespace Template10.Portable.Common
{
    public sealed class Deferral
    {
        private Action _callback;
        public Deferral(Action callback)
        {
            _callback = callback;
        }
        public void Complete()
        {
            _callback.Invoke();
        }
    }
}
