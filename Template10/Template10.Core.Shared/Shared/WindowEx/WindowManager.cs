using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Core
{
    public static class WindowManager
    {
        public static IList<IWindowEx> Instances { get; } = new List<IWindowEx>();
    }
}
