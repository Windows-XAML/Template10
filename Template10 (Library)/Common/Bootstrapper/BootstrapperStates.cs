using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Common
{
    public enum BootstrapperStates
    {
        None,
        Running,
        BeforeInit,
        AfterInit,
        BeforeLaunch,
        AfterLaunch,
        BeforeActivate,
        AfterActivate,
        BeforeStart,
        AfterStart,
    }
}
