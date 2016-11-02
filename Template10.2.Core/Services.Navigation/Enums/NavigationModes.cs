namespace Template10.Services.Navigation
{
    public enum NavigationModes
    {
        New = 0,
        Back = 1,
        Forward = 2,
        Refresh = 3,
        Restore = 4
    }

    public static partial class Extensions
    {
        public static NavigationModes ToNavigationModes(this Windows.UI.Xaml.Navigation.NavigationMode mode)
        {
            switch (mode)
            {
                case Windows.UI.Xaml.Navigation.NavigationMode.Back: return NavigationModes.Back;
                case Windows.UI.Xaml.Navigation.NavigationMode.Forward: return NavigationModes.Forward;
                case Windows.UI.Xaml.Navigation.NavigationMode.Refresh: return NavigationModes.Refresh;
                default: return NavigationModes.New;
            }
        }
    }
}