using Windows.Foundation;
using Windows.UI.Xaml;

namespace Template10
{
    public interface IPrismApplicationEvents
    {
        event EnteredBackgroundEventHandler EnteredBackground;
        event LeavingBackgroundEventHandler LeavingBackground;
        event TypedEventHandler<PrismApplicationBase, WindowCreatedEventArgs> WindowCreated;
    }
}
