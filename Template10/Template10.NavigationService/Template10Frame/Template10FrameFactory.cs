using Windows.UI.Xaml.Controls;

namespace Template10.Services.NavigationService
{
    public static class Template10FrameFactory
    {
        internal static ITemplate10FrameInternal Create(Frame frame, INavigationService navigationService)
        {
            return new Template10Frame(frame, navigationService);
        }
    }
}
