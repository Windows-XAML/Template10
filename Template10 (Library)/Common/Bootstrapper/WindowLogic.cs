using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10.Common
{
    public class WindowLogic
    {
        internal WindowLogic()
        {

        }

        /// <summary>
        /// Override this method only if you (the developer) wants to programmatically
        /// control the means by which and when the Core Window is activated by Template 10.
        /// One scenario might be a delayed activation for Splash Screen.
        /// </summary>
        /// <param name="source">Reason for the call from Template 10</param>
        public void ActivateWindow(ActivateWindowSources source, SplashLogic splashLogic)
        {
            if (source != ActivateWindowSources.SplashScreen)
            {
                splashLogic.Hide();
            }

            Window.Current.Activate();
        }
    }
}
