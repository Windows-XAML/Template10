using Windows.UI.Xaml;

namespace Template10.Common
{
    public class DefaultWindowStrategy : IWindowStrategy
    {
        internal DefaultWindowStrategy()
        {
            // empty
        }

        /// <summary>
        /// Override this method only if you (the developer) wants to programmatically
        /// control the means by which and when the Core Window is activated by Template 10.
        /// One scenario might be a delayed activation for Splash Screen.
        /// </summary>
        /// <param name="source">Reason for the call from Template 10</param>
        public void ActivateWindow(ActivateWindowSources source)
        {
            if (source != ActivateWindowSources.SplashScreen)
            {
                Settings.SplashStrategy.Hide();
            }
            Window.Current.Activate();
        }
    }
}
