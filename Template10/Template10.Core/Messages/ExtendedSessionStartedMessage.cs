using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.ExtendedExecution;

namespace Template10.Messages
{
    public class ExtendedSessionStartedMessage
    {
        public ExtendedExecutionReason ExtendedExecutionReason { get; set; }
    }
}
