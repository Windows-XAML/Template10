using System;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Prism
{
    public interface IPrismApplicationEvents
    {
        event EventHandler<object> Resuming;
        event SuspendingEventHandler Suspending;
        event EnteredBackgroundEventHandler EnteredBackground;
        event LeavingBackgroundEventHandler LeavingBackground;
        event TypedEventHandler<PrismApplicationBase, WindowCreatedEventArgs> WindowCreated;
    }

    public abstract partial class PrismApplicationBase : IPrismApplicationEvents
    {
        EventHandler<object> _resuming;
        event EventHandler<object> IPrismApplicationEvents.Resuming
        {
            add { _resuming += value; }
            remove { _resuming -= value; }
        }
        SuspendingEventHandler _suspending;
        event SuspendingEventHandler IPrismApplicationEvents.Suspending
        {
            add { _suspending += value; }
            remove { _suspending -= value; }
        }
        EnteredBackgroundEventHandler _enteredBackground;
        event EnteredBackgroundEventHandler IPrismApplicationEvents.EnteredBackground
        {
            add { _enteredBackground += value; }
            remove { _enteredBackground -= value; }
        }
        LeavingBackgroundEventHandler _leavingBackground;
        event LeavingBackgroundEventHandler IPrismApplicationEvents.LeavingBackground
        {
            add { _leavingBackground += value; }
            remove { _leavingBackground -= value; }
        }
        TypedEventHandler<PrismApplicationBase, WindowCreatedEventArgs> _windowCreated;
        event TypedEventHandler<PrismApplicationBase, WindowCreatedEventArgs> IPrismApplicationEvents.WindowCreated
        {
            add { _windowCreated += value; }
            remove { _windowCreated -= value; }
        }
    }
}
