using System;

namespace Template10.Common
{
    public interface IBootStrapper
    {
        event EventHandler<HandledEventArgs> BackRequested;
        event EventHandler<HandledEventArgs> ForwardRequested;
    }
}