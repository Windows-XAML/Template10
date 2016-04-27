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

        public void RefreshStyles(ElementTheme theme)
        {
            DebugWrite($"Theme: {theme}");

            if (theme == ElementTheme.Default && RequestedTheme == ElementTheme.Default) RefreshStyles(AccentColor);
            else RefreshStyles(RequestedTheme.ToApplicationTheme());
        }

        public void RefreshStyles(ApplicationTheme theme)
        {
            DebugWrite($"Theme: {theme}");

            RequestedTheme = theme.ToElementTheme();
            RefreshStyles(AccentColor);
        }

        public void RefreshStyles(Color? color = null)
        {
            DebugWrite($"Color: {color}");

            if (color == null) color = AccentColor;
            if (color == default(Color)) return;

            // since every brush will be based on one color,
            // we will do so with theme in mind.

            switch (RequestedTheme)
            {
                case ElementTheme.Light:
                case ElementTheme.Default:
                case ElementTheme.Dark:
                    {
                        this.SetIfNotSet(NavAreaBackgroundProperty, Colors.Gainsboro.Darken(ColorUtils.Add._80p).ToSolidColorBrush());
                        this.SetIfNotSet(SecondarySeparatorProperty, Colors.DimGray.ToSolidColorBrush());
                        this.SetIfNotSet(PaneBorderBrushProperty, Colors.Transparent.ToSolidColorBrush());
                        this.SetIfNotSet(PaneBorderThicknessProperty, new Thickness(0));

                        this.SetIfNotSet(HamburgerForegroundProperty, Colors.White.ToSolidColorBrush());
                        this.SetIfNotSet(HamburgerBackgroundProperty, color?.ToSolidColorBrush());

                        this.SetIfNotSet(NavButtonForegroundProperty, Colors.White.Darken(ColorUtils.Add._20p).ToSolidColorBrush());
                        this.SetIfNotSet(NavButtonBackgroundProperty, Colors.Transparent.ToSolidColorBrush());

                        this.SetIfNotSet(NavButtonCheckedForegroundProperty, Colors.White.ToSolidColorBrush());
                        this.SetIfNotSet(NavButtonCheckedBackgroundProperty, Colors.Transparent.ToSolidColorBrush());
                        this.SetIfNotSet(NavButtonCheckedIndicatorBrushProperty, Colors.White.ToSolidColorBrush());

                        this.SetIfNotSet(NavButtonPressedForegroundProperty, Colors.White.Darken(ColorUtils.Add._30p).ToSolidColorBrush());
                        this.SetIfNotSet(NavButtonPressedBackgroundProperty, Colors.Transparent.ToSolidColorBrush());

                        this.SetIfNotSet(NavButtonHoverForegroundProperty, Colors.White.Darken(ColorUtils.Add._10p).ToSolidColorBrush());
                        this.SetIfNotSet(NavButtonHoverBackgroundProperty, Colors.Transparent.ToSolidColorBrush());
                        break;
                    }
            }
        }
    }
}
