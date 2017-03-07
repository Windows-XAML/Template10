using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10
{
    public static class Locator
    {
        public static ServiceManager<IBootStrapper> BootStrapper { get; } = new ServiceManager<IBootStrapper>();

        public static ServicesManager<IWindowWrapper> WindowWrapper { get; } = new ServicesManager<IWindowWrapper>(m => m.Instances.FirstOrDefault(x => x.Dispatcher.HasThreadAccess()));
    }
}
