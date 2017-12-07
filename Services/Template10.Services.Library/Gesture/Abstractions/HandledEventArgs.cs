using System;

namespace Template10.Services.Gesture
{
    public class HandledEventArgs : EventArgs
    {
        internal HandledEventArgs()
        {
            // empty
        }
        public bool Handled { get; set; }
    }
}