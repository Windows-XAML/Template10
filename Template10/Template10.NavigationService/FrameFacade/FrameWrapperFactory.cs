using Windows.UI.Xaml.Controls;

namespace Template10.Services.NavigationService
{
    public static class FrameWrapperFactory
    {
        internal static IFrameWrapperInternal Create(Frame frame, INavigationService navigationService)
        {
            return new FrameWrapper(frame, navigationService);
        }
    }
}
