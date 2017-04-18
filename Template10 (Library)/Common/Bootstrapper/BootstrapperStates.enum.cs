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
        Launched,
        Activated,
        Prelaunched,
        Restored,
        Started,
        Initialized,
        Launching,
        Activating,
        Starting,
        Prelaunching,
        Initializing,
        Restoring,

        //BeforeInit,
        //AfterInit,
        //BeforeLaunch,
        //AfterLaunch,
        //BeforeActivate,
        //AfterActivate,
        //BeforeStart,
        //AfterStart,
        //BeforePrelaunch,
        //AfterPrelaunch,

    }
}
