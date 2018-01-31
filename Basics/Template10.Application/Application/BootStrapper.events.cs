using System;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Template10.Application
{
    public interface ITemplate10ApplicationEvents
    {
        event EventHandler<object> Resuming;
        event SuspendingEventHandler Suspending;
        event EnteredBackgroundEventHandler EnteredBackground;
        event LeavingBackgroundEventHandler LeavingBackground;
        event TypedEventHandler<BootStrapper, WindowCreatedEventArgs> WindowCreated;
    }

    public abstract partial class BootStrapper : ITemplate10ApplicationEvents
    {
        EventHandler<object> _resuming;
        event EventHandler<object> ITemplate10ApplicationEvents.Resuming
        {
            add { _resuming += value; }
            remove { _resuming -= value; }
        }
        SuspendingEventHandler _suspending;
        event SuspendingEventHandler ITemplate10ApplicationEvents.Suspending
        {
            add { _suspending += value; }
            remove { _suspending -= value; }
        }
        EnteredBackgroundEventHandler _enteredBackground;
        event EnteredBackgroundEventHandler ITemplate10ApplicationEvents.EnteredBackground
        {
            add { _enteredBackground += value; }
            remove { _enteredBackground -= value; }
        }
        LeavingBackgroundEventHandler _leavingBackground;
        event LeavingBackgroundEventHandler ITemplate10ApplicationEvents.LeavingBackground
        {
            add { _leavingBackground += value; }
            remove { _leavingBackground -= value; }
        }
        TypedEventHandler<BootStrapper, WindowCreatedEventArgs> _windowCreated;
        event TypedEventHandler<BootStrapper, WindowCreatedEventArgs> ITemplate10ApplicationEvents.WindowCreated
        {
            add { _windowCreated += value; }
            remove { _windowCreated -= value; }
        }
    }
}
