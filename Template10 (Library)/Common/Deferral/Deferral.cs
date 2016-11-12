using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Common
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
