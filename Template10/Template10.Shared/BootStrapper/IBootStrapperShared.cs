using System;
using System.Collections.Generic;

namespace Template10.Common
{
    public interface IBootStrapperShared
    {
        IDictionary<string, object> SessionState { get; }
    }
}
