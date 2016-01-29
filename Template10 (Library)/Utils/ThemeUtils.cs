using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10.Utils
{
    public static class ThemeUtils
    {
        public static ElementTheme ToElementTheme(this ApplicationTheme theme)
        {
            switch (theme)
            {
                case ApplicationTheme.Light:
                    return ElementTheme.Light;
                case ApplicationTheme.Dark:
                default:
                    return ElementTheme.Dark;
            }
        }

        public static ApplicationTheme ToApplicationTheme(this ElementTheme theme)
        {
            switch (theme)
            {
                case ElementTheme.Default:
                    return ApplicationTheme.Dark;
                case ElementTheme.Light:
                    return ApplicationTheme.Light;
                case ElementTheme.Dark:
                default:
                    return ApplicationTheme.Dark;
            }
        }
    }
}
