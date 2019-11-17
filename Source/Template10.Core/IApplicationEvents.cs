using Windows.UI.Xaml;

namespace Template10
{
    public interface IApplicationEvents
    {
        event EnteredBackgroundEventHandler EnteredBackground;
        event LeavingBackgroundEventHandler LeavingBackground;
        // event TypedEventHandler<ApplicationTemplate, WindowCreatedEventArgs> WindowCreated;
    }
}
