using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.KeyboardService;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    public sealed partial class HamburgerMenu : UserControl
    {
        public event EventHandler PaneOpened;

        public event EventHandler PaneClosed;

        public event EventHandler<ChangedEventArgs<HamburgerButtonInfo>> SelectedChanged;

        public void RefreshStyles(ApplicationTheme? theme = null)
        { //TODO
            DebugWrite($"Theme: {theme}");

            RequestedTheme = theme?.ToElementTheme() ?? RequestedTheme;
            RefreshStyles(AccentColor);
        }

        public void RefreshStyles(Color? color = null)
        {
            DebugWrite($"Color: {color}");

            if (color != null)
            {
                // since every brush will be based on one color,
                // we will do so with theme in mind.

                switch (RequestedTheme)
                {
                    case ElementTheme.Default:
                    case ElementTheme.Light:
                        {
                            HamburgerBackground = color?.ToSolidColorBrush();
                            HamburgerForeground = Colors.White.ToSolidColorBrush();
                            NavAreaBackground = Colors.DimGray.ToSolidColorBrush();
                            NavButtonBackground = Colors.Transparent.ToSolidColorBrush();
                            NavButtonForeground = Colors.White.ToSolidColorBrush();
                            NavButtonCheckedForeground = Colors.White.ToSolidColorBrush();
                            NavButtonCheckedBackground = color?.Lighten(ColorUtils.Accents.Plus20).ToSolidColorBrush();
                            NavButtonPressedBackground = Colors.Gainsboro.Darken(ColorUtils.Accents.Plus40).ToSolidColorBrush();
                            NavButtonHoverBackground = Colors.Gainsboro.Darken(ColorUtils.Accents.Plus60).ToSolidColorBrush();
                            SecondarySeparator = PaneBorderBrush = Colors.Gainsboro.Darken(ColorUtils.Accents.Plus40).ToSolidColorBrush();
                        }
                        break;
                    case ElementTheme.Dark:
                        {
                            HamburgerBackground = color?.ToSolidColorBrush();
                            HamburgerForeground = Colors.White.ToSolidColorBrush();
                            NavAreaBackground = Colors.Gainsboro.Darken(ColorUtils.Accents.Plus80).ToSolidColorBrush();
                            NavButtonBackground = Colors.Transparent.ToSolidColorBrush();
                            NavButtonForeground = Colors.White.ToSolidColorBrush();
                            NavButtonCheckedForeground = Colors.White.ToSolidColorBrush();
                            NavButtonCheckedBackground = color?.Darken(ColorUtils.Accents.Plus40).ToSolidColorBrush();
                            NavButtonPressedBackground = Colors.Gainsboro.Lighten(ColorUtils.Accents.Plus40).ToSolidColorBrush();
                            NavButtonHoverBackground = Colors.Gainsboro.Lighten(ColorUtils.Accents.Plus60).ToSolidColorBrush();
                            SecondarySeparator = PaneBorderBrush = Colors.Gainsboro.ToSolidColorBrush();
                        }
                        break;
                }
            }
        }
    }
}
