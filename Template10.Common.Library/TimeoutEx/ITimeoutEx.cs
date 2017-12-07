using System;

namespace Template10.Common
{
    public interface ITimeoutEx
    {
        event EventHandler Tick;

        void Start(Action callback);
        void Stop();
    }
}