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

        public void RefreshStyles(ApplicationTheme theme)
        {
            DebugWrite($"Theme: {theme}");

            RequestedTheme = theme.ToElementTheme();
            RefreshStyles(AccentColor);
        }

        public void RefreshStyles(Color? color = null)
        {
            DebugWrite($"Color: {color}");

            if (color == null)
                color = AccentColor;

            // since every brush will be based on one color,
            // we will do so with theme in mind.

            switch (RequestedTheme)
            {
                case ElementTheme.Light:
                case ElementTheme.Default:
                case ElementTheme.Dark:
                    {
                        NavAreaBackground = Colors.Gainsboro.Darken(ColorUtils.Add._80p).ToSolidColorBrush();
                        SecondarySeparator = Colors.DimGray.ToSolidColorBrush();
                        PaneBorderBrush = Colors.Transparent.ToSolidColorBrush();

                        HamburgerForeground = Colors.White.ToSolidColorBrush();
                        HamburgerBackground = color?.ToSolidColorBrush();

                        NavButtonForeground = Colors.White.Darken(ColorUtils.Add._20p).ToSolidColorBrush();
                        NavButtonBackground = Colors.Transparent.ToSolidColorBrush();

                        NavButtonCheckedForeground = Colors.White.ToSolidColorBrush();
                        NavButtonCheckedBackground = Colors.Transparent.ToSolidColorBrush();
                        NavButtonCheckedIndicatorBrush = Colors.White.ToSolidColorBrush();

                        NavButtonPressedForeground = Colors.White.Darken(ColorUtils.Add._30p).ToSolidColorBrush();
                        NavButtonPressedBackground = Colors.Transparent.ToSolidColorBrush();

                        NavButtonHoverForeground = Colors.White.Darken(ColorUtils.Add._10p).ToSolidColorBrush();
                        NavButtonHoverBackground = Colors.Transparent.ToSolidColorBrush();
                        break;
                    }
            }
        }
    }
}
