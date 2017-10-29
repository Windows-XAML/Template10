using Template10.Common;

namespace Template10.Services.Gesture
{
    public interface IBackButtonService2: IBackButtonService
    {
        void Setup();
        HandledEventArgs RaiseBackRequested();
        HandledEventArgs RaiseForwardRequested();
    }
}