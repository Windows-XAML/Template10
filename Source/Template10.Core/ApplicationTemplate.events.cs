using System;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace Template10
{
    public abstract partial class ApplicationTemplate : IApplicationEvents
    {
#pragma warning disable CS0067 // unused events
        [EditorBrowsable(EditorBrowsableState.Never)]
        private new event EventHandler<object> Resuming;
        [EditorBrowsable(EditorBrowsableState.Never)]
        private new event SuspendingEventHandler Suspending;
        [EditorBrowsable(EditorBrowsableState.Never)]
        private new event EnteredBackgroundEventHandler EnteredBackground;
        [EditorBrowsable(EditorBrowsableState.Never)]
        private new event LeavingBackgroundEventHandler LeavingBackground;
#pragma warning restore CS0067

        private EnteredBackgroundEventHandler _enteredBackground;
        event EnteredBackgroundEventHandler IApplicationEvents.EnteredBackground
        {
            add { _enteredBackground += value; }
            remove { _enteredBackground -= value; }
        }

        private LeavingBackgroundEventHandler _leavingBackground;
        event LeavingBackgroundEventHandler IApplicationEvents.LeavingBackground
        {
            add { _leavingBackground += value; }
            remove { _leavingBackground -= value; }
        }
        //TypedEventHandler<ApplicationTemplate, WindowCreatedEventArgs> _windowCreated;
        //event TypedEventHandler<ApplicationTemplate, WindowCreatedEventArgs> IApplicationEvents.WindowCreated
        //{
        //    add { _windowCreated += value; }
        //    remove { _windowCreated -= value; }
        //}
    }
}
