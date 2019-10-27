namespace Template10.Navigation
{
    public static class NavigationParametersExtensions
    {
        public static void Remove(this INavigationParametersInternal nav, string key)
        {
            (nav as NavigationParameters)._internal.Remove(key);
        }
    }
}
