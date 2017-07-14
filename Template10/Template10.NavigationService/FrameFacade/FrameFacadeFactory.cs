using Windows.UI.Xaml.Controls;

namespace Template10.Services.NavigationService
{
    public static class FrameFacadeFactory
    {
        internal static IFrameFacadeInternal Create(Frame frame, INavigationService navigationService)
        {
            return new FrameFacade(frame, navigationService);
        }
    }
}
