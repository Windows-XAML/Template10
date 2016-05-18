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
    internal static class Extensions
    {
        public static ChangedEventArgs<T> ToChangedEventArgs<T>(this DependencyPropertyChangedEventArgs e)
            => new ChangedEventArgs<T>((T)e.OldValue, (T)e.NewValue);
    }

    public sealed partial class HamburgerMenu : UserControl
    {
        #region style properties

        public Brush NavAreaBackground
        {
            get { return GetValue(NavAreaBackgroundProperty) as Brush; }
            set { SetValue(NavAreaBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavAreaBackgroundProperty =
              DependencyProperty.Register(nameof(NavAreaBackground), typeof(Brush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                  {
                      WriteDebug(nameof(NavAreaBackground), e);
                      (d as HamburgerMenu).NavAreaBackgroundChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                      (d as HamburgerMenu).InternalNavAreaBackgroundChanged(e.ToChangedEventArgs<Brush>());
                  }));
        public event EventHandler<ChangedEventArgs<Brush>> NavAreaBackgroundChanged;
        partial void InternalNavAreaBackgroundChanged(ChangedEventArgs<Brush> e);

        public Brush SecondarySeparator
        {
            get { return GetValue(SecondarySeparatorProperty) as Brush; }
            set { SetValue(SecondarySeparatorProperty, value); }
        }
        public static readonly DependencyProperty SecondarySeparatorProperty =
              DependencyProperty.Register(nameof(SecondarySeparator), typeof(Brush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                  {
                      WriteDebug(nameof(SecondarySeparator), e);
                      (d as HamburgerMenu).SecondarySeparatorChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                      (d as HamburgerMenu).InternalSecondarySeparatorChanged(e.ToChangedEventArgs<Brush>());
                  }));
        public event EventHandler<ChangedEventArgs<Brush>> SecondarySeparatorChanged;
        partial void InternalSecondarySeparatorChanged(ChangedEventArgs<Brush> e);

        public Brush PaneBorderBrush
        {
            get { return GetValue(PaneBorderBrushProperty) as Brush; }
            set { SetValue(PaneBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty PaneBorderBrushProperty =
              DependencyProperty.Register(nameof(PaneBorderBrush), typeof(Brush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                  {
                      WriteDebug(nameof(PaneBorderBrush), e);
                      (d as HamburgerMenu).PaneBorderBrushChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                      (d as HamburgerMenu).InternalPaneBorderBrushChanged(e.ToChangedEventArgs<Brush>());
                  }));
        public event EventHandler<ChangedEventArgs<Brush>> PaneBorderBrushChanged;
        partial void InternalPaneBorderBrushChanged(ChangedEventArgs<Brush> e);


        // ham button

        public Brush HamburgerForeground
        {
            get { return GetValue(HamburgerForegroundProperty) as Brush; }
            set { SetValue(HamburgerForegroundProperty, value); }
        }
        public static readonly DependencyProperty HamburgerForegroundProperty =
              DependencyProperty.Register(nameof(HamburgerForeground), typeof(Brush),
                  typeof(HamburgerMenu), new PropertyMetadata(Colors.White.ToSolidColorBrush(), (d, e) =>
                  {
                      WriteDebug(nameof(HamburgerForeground), e);
                      (d as HamburgerMenu).HamburgerForegroundChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                      (d as HamburgerMenu).InternalHamburgerForegroundChanged(e.ToChangedEventArgs<Brush>());
                  }));
        public event EventHandler<ChangedEventArgs<Brush>> HamburgerForegroundChanged;
        partial void InternalHamburgerForegroundChanged(ChangedEventArgs<Brush> e);


        public Brush HamburgerBackground
        {
            get { return GetValue(HamburgerBackgroundProperty) as Brush; }
            set { SetValue(HamburgerBackgroundProperty, value); }
        }
        public static readonly DependencyProperty HamburgerBackgroundProperty =
            DependencyProperty.Register(nameof(HamburgerBackground), typeof(Brush),
                typeof(HamburgerMenu), new PropertyMetadata(Colors.SteelBlue.ToSolidColorBrush(), (d, e) =>
                {
                    WriteDebug(nameof(HamburgerBackground), e);
                    (d as HamburgerMenu).HamburgerBackgroundChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                    (d as HamburgerMenu).InternalHamburgerBackgroundChanged(e.ToChangedEventArgs<Brush>());
                }));
        public event EventHandler<ChangedEventArgs<Brush>> HamburgerBackgroundChanged;
        partial void InternalHamburgerBackgroundChanged(ChangedEventArgs<Brush> e);


        // nav button | normal

        public Brush NavButtonForeground
        {
            get { return GetValue(NavButtonForegroundProperty) as Brush; }
            set { SetValue(NavButtonForegroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonForegroundProperty =
            DependencyProperty.Register(nameof(NavButtonForeground), typeof(Brush),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    WriteDebug(nameof(NavButtonForeground), e);
                    (d as HamburgerMenu).NavButtonForegroundChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                    (d as HamburgerMenu).InternalNavButtonForegroundChanged(e.ToChangedEventArgs<Brush>());
                }));
        public event EventHandler<ChangedEventArgs<Brush>> NavButtonForegroundChanged;
        partial void InternalNavButtonForegroundChanged(ChangedEventArgs<Brush> e);


        public Brush NavButtonBackground
        {
            get { return GetValue(NavButtonBackgroundProperty) as Brush; }
            set { SetValue(NavButtonBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonBackgroundProperty =
            DependencyProperty.Register(nameof(NavButtonBackground), typeof(Brush),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    WriteDebug(nameof(NavButtonBackground), e);
                    (d as HamburgerMenu).NavButtonBackgroundChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                    (d as HamburgerMenu).InternalNavButtonBackgroundChanged(e.ToChangedEventArgs<Brush>());
                }));
        public event EventHandler<ChangedEventArgs<Brush>> NavButtonBackgroundChanged;
        partial void InternalNavButtonBackgroundChanged(ChangedEventArgs<Brush> e);


        // nav button | checked

        public Brush NavButtonCheckedForeground
        {
            get { return GetValue(NavButtonCheckedForegroundProperty) as Brush; }
            set { SetValue(NavButtonCheckedForegroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonCheckedForegroundProperty =
              DependencyProperty.Register(nameof(NavButtonCheckedForeground), typeof(Brush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                  {
                      WriteDebug(nameof(NavButtonCheckedForeground), e);
                      (d as HamburgerMenu).NavButtonCheckedForegroundChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                      (d as HamburgerMenu).InternalNavButtonCheckedForegroundChanged(e.ToChangedEventArgs<Brush>());
                  }));
        public event EventHandler<ChangedEventArgs<Brush>> NavButtonCheckedForegroundChanged;
        partial void InternalNavButtonCheckedForegroundChanged(ChangedEventArgs<Brush> e);


        public Brush NavButtonCheckedBackground
        {
            get { return GetValue(NavButtonCheckedBackgroundProperty) as Brush; }
            set { SetValue(NavButtonCheckedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonCheckedBackgroundProperty =
              DependencyProperty.Register(nameof(NavButtonCheckedBackground), typeof(Brush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                  {
                      WriteDebug(nameof(NavButtonCheckedBackground), e);
                      (d as HamburgerMenu).NavButtonCheckedBackgroundChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                      (d as HamburgerMenu).InternalNavButtonCheckedBackgroundChanged(e.ToChangedEventArgs<Brush>());
                  }));
        public event EventHandler<ChangedEventArgs<Brush>> NavButtonCheckedBackgroundChanged;
        partial void InternalNavButtonCheckedBackgroundChanged(ChangedEventArgs<Brush> e);


        public Brush NavButtonCheckedIndicatorBrush
        {
            get { return GetValue(NavButtonCheckedIndicatorBrushProperty) as Brush; }
            set { SetValue(NavButtonCheckedIndicatorBrushProperty, value); }
        }
        public static readonly DependencyProperty NavButtonCheckedIndicatorBrushProperty =
              DependencyProperty.Register(nameof(NavButtonCheckedIndicatorBrush), typeof(Brush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                  {
                      WriteDebug(nameof(NavButtonCheckedIndicatorBrush), e);
                      (d as HamburgerMenu).NavButtonCheckedIndicatorBrushChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                      (d as HamburgerMenu).InternalNavButtonCheckedIndicatorBrushChanged(e.ToChangedEventArgs<Brush>());
                  }));
        public event EventHandler<ChangedEventArgs<Brush>> NavButtonCheckedIndicatorBrushChanged;
        partial void InternalNavButtonCheckedIndicatorBrushChanged(ChangedEventArgs<Brush> e);


        // nav button | pressed

        public Brush NavButtonPressedForeground
        {
            get { return GetValue(NavButtonPressedForegroundProperty) as Brush; }
            set { SetValue(NavButtonPressedForegroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonPressedForegroundProperty =
              DependencyProperty.Register(nameof(NavButtonPressedForeground), typeof(Brush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                  {
                      WriteDebug(nameof(NavButtonPressedForeground), e);
                      (d as HamburgerMenu).NavButtonPressedForegroundChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                      (d as HamburgerMenu).InternalNavButtonPressedForegroundChanged(e.ToChangedEventArgs<Brush>());
                  }));
        public event EventHandler<ChangedEventArgs<Brush>> NavButtonPressedForegroundChanged;
        partial void InternalNavButtonPressedForegroundChanged(ChangedEventArgs<Brush> e);


        public Brush NavButtonPressedBackground
        {
            get { return GetValue(NavButtonPressedBackgroundProperty) as Brush; }
            set { SetValue(NavButtonPressedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonPressedBackgroundProperty =
              DependencyProperty.Register(nameof(NavButtonPressedBackground), typeof(Brush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                  {
                      WriteDebug(nameof(NavButtonPressedBackground), e);
                      (d as HamburgerMenu).NavButtonPressedBackgroundChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                      (d as HamburgerMenu).InternalNavButtonPressedBackgroundChanged(e.ToChangedEventArgs<Brush>());
                  }));
        public event EventHandler<ChangedEventArgs<Brush>> NavButtonPressedBackgroundChanged;
        partial void InternalNavButtonPressedBackgroundChanged(ChangedEventArgs<Brush> e);


        // nav button | hover

        public Brush NavButtonHoverForeground
        {
            get { return GetValue(NavButtonHoverForegroundProperty) as Brush; }
            set { SetValue(NavButtonHoverForegroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonHoverForegroundProperty =
              DependencyProperty.Register(nameof(NavButtonHoverForeground), typeof(Brush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                  {
                      WriteDebug(nameof(NavButtonHoverForeground), e);
                      (d as HamburgerMenu).NavButtonHoverForegroundChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                      (d as HamburgerMenu).InternalNavButtonHoverForegroundChanged(e.ToChangedEventArgs<Brush>());
                  }));
        public event EventHandler<ChangedEventArgs<Brush>> NavButtonHoverForegroundChanged;
        partial void InternalNavButtonHoverForegroundChanged(ChangedEventArgs<Brush> e);


        public Brush NavButtonHoverBackground
        {
            get { return GetValue(NavButtonHoverBackgroundProperty) as Brush; }
            set { SetValue(NavButtonHoverBackgroundProperty, value); }
        }
        public static readonly DependencyProperty NavButtonHoverBackgroundProperty =
              DependencyProperty.Register(nameof(NavButtonHoverBackground), typeof(Brush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                  {
                      WriteDebug(nameof(NavButtonHoverBackground), e);
                      (d as HamburgerMenu).NavButtonHoverBackgroundChanged?.Invoke(d, e.ToChangedEventArgs<Brush>());
                      (d as HamburgerMenu).InternalNavButtonHoverBackgroundChanged(e.ToChangedEventArgs<Brush>());
                  }));
        public event EventHandler<ChangedEventArgs<Brush>> NavButtonHoverBackgroundChanged;
        partial void InternalNavButtonHoverBackgroundChanged(ChangedEventArgs<Brush> e);


        #endregion

        /// <summary>
        /// Gets or sets the visibility of the Hamburger button. This is generally automatically set by the visual states,
        /// but can manually be set to accomodate custom layouts.
        /// </summary>
        public Visibility HamburgerButtonVisibility
        {
            get { return (Visibility)GetValue(HamburgerButtonVisibilityProperty); }
            set { SetValue(HamburgerButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty HamburgerButtonVisibilityProperty =
            DependencyProperty.Register(nameof(HamburgerButtonVisibility), typeof(Visibility),
                typeof(HamburgerMenu), new PropertyMetadata(Visibility.Visible, (d, e) =>
                {
                    WriteDebug(nameof(HamburgerButtonVisibility), e);
                    (d as HamburgerMenu).HamburgerButtonVisibilityChanged?.Invoke(d, e.ToChangedEventArgs<Visibility>());
                    (d as HamburgerMenu).InternalHamburgerButtonVisibilityChanged(e.ToChangedEventArgs<Visibility>());
                }));
        public event EventHandler<ChangedEventArgs<Visibility>> HamburgerButtonVisibilityChanged;
        partial void InternalHamburgerButtonVisibilityChanged(ChangedEventArgs<Visibility> e);


        /// <summary>
        /// DisplayMode is a projection of the DisplayMode property in the underlying SplitView. Setting this property
        /// will set the property in the underlying SplitView. This is generally automatically set by the visual states,
        /// but can manually be set to accomodate custom layouts.
        /// </summary>
        public SplitViewDisplayMode DisplayMode
        {
            get { return (SplitViewDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }
        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register(nameof(DisplayMode), typeof(SplitViewDisplayMode),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    WriteDebug(nameof(DisplayMode), e);
                    (d as HamburgerMenu).DisplayModeChanged?.Invoke(d, e.ToChangedEventArgs<SplitViewDisplayMode>());
                    (d as HamburgerMenu).InternalDisplayModeChanged(e.ToChangedEventArgs<SplitViewDisplayMode>());
                }));
        public event EventHandler<ChangedEventArgs<SplitViewDisplayMode>> DisplayModeChanged;
        partial void InternalDisplayModeChanged(ChangedEventArgs<SplitViewDisplayMode> e);


        public SplitViewDisplayMode VisualStateNarrowDisplayMode
        {
            get { return (SplitViewDisplayMode)GetValue(VisualStateNarrowDisplayModeProperty); }
            set { SetValue(VisualStateNarrowDisplayModeProperty, value); }
        }
        public static readonly DependencyProperty VisualStateNarrowDisplayModeProperty =
            DependencyProperty.Register(nameof(VisualStateNarrowDisplayMode), typeof(SplitViewDisplayMode),
                typeof(HamburgerMenu), new PropertyMetadata(SplitViewDisplayMode.Overlay, (d, e) =>
                {
                    WriteDebug(nameof(VisualStateNarrowDisplayMode), e);
                    (d as HamburgerMenu).VisualStateNarrowDisplayModeChanged?.Invoke(d, e.ToChangedEventArgs<SplitViewDisplayMode>());
                    (d as HamburgerMenu).InternalVisualStateNarrowDisplayModeChanged(e.ToChangedEventArgs<SplitViewDisplayMode>());
                }));
        public event EventHandler<ChangedEventArgs<SplitViewDisplayMode>> VisualStateNarrowDisplayModeChanged;
        partial void InternalVisualStateNarrowDisplayModeChanged(ChangedEventArgs<SplitViewDisplayMode> e);


        public SplitViewDisplayMode VisualStateNormalDisplayMode
        {
            get { return (SplitViewDisplayMode)GetValue(VisualStateNormalDisplayModeProperty); }
            set { SetValue(VisualStateNormalDisplayModeProperty, value); }
        }
        public static readonly DependencyProperty VisualStateNormalDisplayModeProperty =
            DependencyProperty.Register(nameof(VisualStateNormalDisplayMode), typeof(SplitViewDisplayMode),
                typeof(HamburgerMenu), new PropertyMetadata(SplitViewDisplayMode.CompactOverlay, (d, e) =>
                {
                    WriteDebug(nameof(VisualStateNormalDisplayMode), e);
                    (d as HamburgerMenu).VisualStateNormalDisplayModeChanged?.Invoke(d, e.ToChangedEventArgs<SplitViewDisplayMode>());
                    (d as HamburgerMenu).InternalVisualStateNormalDisplayModeChanged(e.ToChangedEventArgs<SplitViewDisplayMode>());
                }));
        public event EventHandler<ChangedEventArgs<SplitViewDisplayMode>> VisualStateNormalDisplayModeChanged;
        partial void InternalVisualStateNormalDisplayModeChanged(ChangedEventArgs<SplitViewDisplayMode> e);


        public SplitViewDisplayMode VisualStateWideDisplayMode
        {
            get { return (SplitViewDisplayMode)GetValue(VisualStateWideDisplayModeProperty); }
            set { SetValue(VisualStateWideDisplayModeProperty, value); }
        }
        public static readonly DependencyProperty VisualStateWideDisplayModeProperty =
            DependencyProperty.Register(nameof(VisualStateWideDisplayMode), typeof(SplitViewDisplayMode),
                typeof(HamburgerMenu), new PropertyMetadata(SplitViewDisplayMode.CompactInline, (d, e) =>
                {
                    WriteDebug(nameof(VisualStateWideDisplayMode), e);
                    (d as HamburgerMenu).VisualStateWideDisplayModeChanged?.Invoke(d, e.ToChangedEventArgs<SplitViewDisplayMode>());
                    (d as HamburgerMenu).InternalVisualStateWideDisplayModeChanged(e.ToChangedEventArgs<SplitViewDisplayMode>());
                }));
        public event EventHandler<ChangedEventArgs<SplitViewDisplayMode>> VisualStateWideDisplayModeChanged;
        partial void InternalVisualStateWideDisplayModeChanged(ChangedEventArgs<SplitViewDisplayMode> e);


        /// <summary>
        /// This is one of three visual state properties. It sets the minimum value used to invoke the Wide visual state.
        /// </summary>
        /// <remarks>
        /// In the Template 10 world, there are typically three visual states: 1) Minimal, 2) Normal, and 3) Wide
        /// </remarks>
        public double VisualStateNarrowMinWidth
        {
            get { return (double)GetValue(VisualStateNarrowMinWidthProperty); }
            set { SetValue(VisualStateNarrowMinWidthProperty, value); }
        }
        public static readonly DependencyProperty VisualStateNarrowMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNarrowMinWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata((double)-1, (d, e) =>
                {
                    WriteDebug(nameof(VisualStateNarrowMinWidth), e);
                    (d as HamburgerMenu).VisualStateNarrowMinWidthChanged?.Invoke(d, e.ToChangedEventArgs<double>());
                    (d as HamburgerMenu).InternalVisualStateNarrowMinWidthChanged(e.ToChangedEventArgs<double>());
                }));
        public event EventHandler<ChangedEventArgs<double>> VisualStateNarrowMinWidthChanged;
        partial void InternalVisualStateNarrowMinWidthChanged(ChangedEventArgs<double> e);


        /// <summary>
        /// This is one of three visual state properties. It sets the minimum value used to invoke the Normal visual state.
        /// </summary>
        /// <remarks>
        /// In the Template 10 world, there are typically three visual states: 1) Minimal, 2) Normal, and 3) Wide
        /// </remarks>
        public double VisualStateNormalMinWidth
        {
            get { return (double)GetValue(VisualStateNormalMinWidthProperty); }
            set { SetValue(VisualStateNormalMinWidthProperty, value); }
        }
        public static readonly DependencyProperty VisualStateNormalMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNormalMinWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata((double)-1, (d, e) =>
                {
                    WriteDebug(nameof(VisualStateNormalMinWidth), e);
                    (d as HamburgerMenu).VisualStateNormalMinWidthChanged?.Invoke(d, e.ToChangedEventArgs<double>());
                    (d as HamburgerMenu).InternalVisualStateNormalMinWidthChanged(e.ToChangedEventArgs<double>());
                }));
        public event EventHandler<ChangedEventArgs<double>> VisualStateNormalMinWidthChanged;
        partial void InternalVisualStateNormalMinWidthChanged(ChangedEventArgs<double> e);


        /// <summary>
        /// This is one of three visual state properties. It sets the minimum value used to invoke the Minimum visual state.
        /// </summary>
        /// <remarks>
        /// In the Template 10 world, there are typically three visual states: 1) Minimal, 2) Normal, and 3) Wide
        /// </remarks>
        public double VisualStateWideMinWidth
        {
            get { return (double)GetValue(VisualStateWideMinWidthProperty); }
            set { SetValue(VisualStateWideMinWidthProperty, value); }
        }
        public static readonly DependencyProperty VisualStateWideMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateWideMinWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata((double)-1, (d, e) =>
                {
                    WriteDebug(nameof(VisualStateWideMinWidth), e);
                    (d as HamburgerMenu).VisualStateWideMinWidthChanged?.Invoke(d, e.ToChangedEventArgs<double>());
                    (d as HamburgerMenu).InternalVisualStateWideMinWidthChanged(e.ToChangedEventArgs<double>());
                }));
        public event EventHandler<ChangedEventArgs<double>> VisualStateWideMinWidthChanged;
        partial void InternalVisualStateWideMinWidthChanged(ChangedEventArgs<double> e);


        /// <summary>
        /// This sets or gets if the bottom buttons in the Hamburger Menu are arranged vertically or horizontally.
        /// The value of this property will only be effective when the IsOpen property is true and the DisplayMode is
        /// not in Compact mode. In the inverse of that combination, buttons will be Vertical because nothing else makes sense.
        /// </summary>
        public Orientation SecondaryButtonOrientation
        {
            get { return (Orientation)GetValue(SecondaryButtonOrientationProperty); }
            set { SetValue(SecondaryButtonOrientationProperty, value); }
        }
        public static readonly DependencyProperty SecondaryButtonOrientationProperty =
            DependencyProperty.Register(nameof(SecondaryButtonOrientation), typeof(Orientation),
                typeof(HamburgerMenu), new PropertyMetadata(Orientation.Vertical, (d, e) =>
                {
                    WriteDebug(nameof(SecondaryButtonOrientation), e);
                    (d as HamburgerMenu).SecondaryButtonOrientationChanged?.Invoke(d, e.ToChangedEventArgs<Orientation>());
                    (d as HamburgerMenu).InternalSecondaryButtonOrientationChanged(e.ToChangedEventArgs<Orientation>());
                }));
        public event EventHandler<ChangedEventArgs<Orientation>> SecondaryButtonOrientationChanged;
        partial void InternalSecondaryButtonOrientationChanged(ChangedEventArgs<Orientation> e);


        /// <summary>
        /// There are several color properties in the Hamburger Menu control, however, the AccentColor property
        /// is intended to make setting all those properties easier. Setting AccentColor will set the value of
        /// all the style-related properties in the menu. However, one consequence is that it will overwrite
        /// any existing values. If you desire to set properties individually, do not set AccentColor.
        /// </summary>
        public Color AccentColor
        {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }
        public static readonly DependencyProperty AccentColorProperty =
            DependencyProperty.Register(nameof(AccentColor), typeof(Color),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    WriteDebug(nameof(AccentColor), e);
                    (d as HamburgerMenu).AccentColorChanged?.Invoke(d, e.ToChangedEventArgs<Color>());
                    (d as HamburgerMenu).InternalAccentColorChanged(e.ToChangedEventArgs<Color>());
                }));
        public event EventHandler<ChangedEventArgs<Color>> AccentColorChanged;
        partial void InternalAccentColorChanged(ChangedEventArgs<Color> e);


        /// <summary>
        /// Selected indicates the current button highlighted in the navigation Pane. Setting this
        /// value will highlight the corresponding button as well as invoke navigation.
        /// </summary>
        public HamburgerButtonInfo Selected
        {
            get { return GetValue(SelectedProperty) as HamburgerButtonInfo; }
            set { SetValue(SelectedProperty, value); }
        }
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register(nameof(Selected), typeof(HamburgerButtonInfo),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    WriteDebug(nameof(Selected), e);
                    (d as HamburgerMenu).SelectedChanged?.Invoke(d, e.ToChangedEventArgs<HamburgerButtonInfo>());
                    (d as HamburgerMenu).InternalSelectedChanged(e.ToChangedEventArgs<HamburgerButtonInfo>());
                }));
        public event EventHandler<ChangedEventArgs<HamburgerButtonInfo>> SelectedChanged;
        partial void InternalSelectedChanged(ChangedEventArgs<HamburgerButtonInfo> e);


        /// <summary>
        /// The Navigation Service is used to synchronize the hamburger buttons in the navigation Pane
        /// with the location in the Frame. In addition, it is used to invoke navigation when the user
        /// clicks on a hamburger info button in the navigation Pane.
        /// </summary>
        /// <remarks>
        /// It is impossible for the Templat 10 Hamburger Button to function without a Navigation Service.
        /// As a result, it should be set as early in the pipeline as possible.
        /// </remarks>
        public INavigationService NavigationService
        {
            get { return (INavigationService)GetValue(NavigationServiceProperty); }
            set { SetValue(NavigationServiceProperty, value); }
        }
        public static readonly DependencyProperty NavigationServiceProperty =
            DependencyProperty.Register(nameof(NavigationService), typeof(INavigationService),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    WriteDebug(nameof(NavigationService), e);
                    (d as HamburgerMenu).NavigationServiceChanged?.Invoke(d, e.ToChangedEventArgs<INavigationService>());
                    (d as HamburgerMenu).InternalNavigationServiceChanged(e.ToChangedEventArgs<INavigationService>());
                }));
        public event EventHandler<ChangedEventArgs<INavigationService>> NavigationServiceChanged;
        partial void InternalNavigationServiceChanged(ChangedEventArgs<INavigationService> e);


        /// <summary>
        /// This toggles if the navigation Pane is visible or hidden. This includes the Hamburger button, too.
        /// A common use case for this property is to show media elements full screen. It is used internally
        /// to show the splash screen full screen during load.
        /// </summary>
        public bool IsFullScreen
        {
            get { return (bool)GetValue(IsFullScreenProperty); }
            set { SetValue(IsFullScreenProperty, value); }
        }
        public static readonly DependencyProperty IsFullScreenProperty =
            DependencyProperty.Register(nameof(IsFullScreen), typeof(bool),
                typeof(HamburgerMenu), new PropertyMetadata(false, (d, e) =>
                {
                    WriteDebug(nameof(IsFullScreen), e);
                    (d as HamburgerMenu).IsFullScreenChanged?.Invoke(d, e.ToChangedEventArgs<bool>());
                    (d as HamburgerMenu).InternalIsFullScreenChanged(e.ToChangedEventArgs<bool>());
                }));
        public event EventHandler<ChangedEventArgs<bool>> IsFullScreenChanged;
        partial void InternalIsFullScreenChanged(ChangedEventArgs<bool> e);


        /// <summary>
        /// PaneWidth indicates the width of the Pane when it is open. The width of the Pane
        /// when it is closed is hard-coded to 48 pixels. 
        /// </summary>
        /// <remarks>
        /// The reason the closed width of the pane is hard-coded to 48 pixels is because this
        /// matches the closed width of the MSN News app, after which we modeled this control.
        /// </remarks>
        public double PaneWidth
        {
            get { return (double)GetValue(PaneWidthProperty); }
            set { SetValue(PaneWidthProperty, value); }
        }
        public static readonly DependencyProperty PaneWidthProperty =
            DependencyProperty.Register(nameof(PaneWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata(220d, (d, e) =>
                {
                    WriteDebug(nameof(PaneWidth), e);
                    (d as HamburgerMenu).PaneWidthChanged?.Invoke(d, e.ToChangedEventArgs<double>());
                    (d as HamburgerMenu).InternalPaneWidthChanged(e.ToChangedEventArgs<double>());
                }));
        public event EventHandler<ChangedEventArgs<double>> PaneWidthChanged;
        partial void InternalPaneWidthChanged(ChangedEventArgs<double> e);


        /// <summary>
        /// HamburgerButtonGridWidth represents the width of a Grid containing 
        /// the HamburgerMenu button.
        /// </summary>
        /// <remarks>
        /// The Grid width must remain the same size as the HamburgerMenu button (48px wide)
        /// except when (ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactInline) &&
        /// (ShellSplitView.IsPaneOpen == true), in which case it must adjust to the width of 
        /// PaneWidth to fill in the gap between HamburgerMenu button and PageHeader.
        /// Previous implementation applied the HamburgerMenu background brush 
        /// to the RootGrid control but this rather easy approach had its drawback of momentarily
        /// showing the page-wide RootGrid background while changing the dark/light theme (as noticeable flash 
        /// for bright colors such as variants of prime colors). With this adaptive 
        /// HamburgerButtonGridWidth, the area is just a narrow strip (as opposed to page-wide 
        /// RootGrid control) and the the flashing problem is virtually non-existent.
        /// </remarks>
        public double HamburgerButtonGridWidth
        {
            get { return (double)GetValue(HamburgerButtonGridWidthProperty); }
            set { SetValue(HamburgerButtonGridWidthProperty, value); }
        }
        public static readonly DependencyProperty HamburgerButtonGridWidthProperty =
            DependencyProperty.Register(nameof(HamburgerButtonGridWidth), typeof(double),
               typeof(HamburgerMenu), new PropertyMetadata(48d, (d, e) =>
               {
                   WriteDebug(nameof(HamburgerButtonGridWidth), e);
                   (d as HamburgerMenu).HamburgerButtonGridWidthChanged?.Invoke(d, e.ToChangedEventArgs<double>());
                   (d as HamburgerMenu).InternalHamburgerButtonGridWidthChanged(e.ToChangedEventArgs<double>());
               }));
        public event EventHandler<ChangedEventArgs<double>> HamburgerButtonGridWidthChanged;
        partial void InternalHamburgerButtonGridWidthChanged(ChangedEventArgs<double> e);


        [Flags]
        public enum OpenCloseModes { None = 1, Auto = 2, Tap = 4, Swipe = 5 }

        /// <summary>
        /// The Hamburger Menu can can respond to several gestures to toggle the IsOpen property
        /// and show the navigation Pane. The default is Auto, which basically means Both (or All).
        /// </summary>
        /// <remarks>
        /// Specifically, Tap means you can tap the empty area of the Hamburger Menu to open or
        /// close the navigation Pane. Swip means a left->right gesture will reveal it and a
        /// right->left gesture will hide it. 
        /// </remarks>
        public OpenCloseModes OpenCloseMode
        {
            get { return (OpenCloseModes)GetValue(OpenCloseModeProperty); }
            set { SetValue(OpenCloseModeProperty, value); }
        }
        public static readonly DependencyProperty OpenCloseModeProperty =
            DependencyProperty.Register(nameof(OpenCloseMode), typeof(OpenCloseModes),
                typeof(HamburgerMenu), new PropertyMetadata(OpenCloseModes.Auto, (d, e) =>
                {
                    WriteDebug(nameof(OpenCloseMode), e);
                    (d as HamburgerMenu).OpenCloseModeChanged?.Invoke(d, e.ToChangedEventArgs<OpenCloseModes>());
                    (d as HamburgerMenu).InternalOpenCloseModeChanged(e.ToChangedEventArgs<OpenCloseModes>());
                }));
        public event EventHandler<ChangedEventArgs<OpenCloseModes>> OpenCloseModeChanged;
        partial void InternalOpenCloseModeChanged(ChangedEventArgs<OpenCloseModes> e);



        /// <summary>
        /// The Panel border thickness is intended to be the border between between the open
        /// pane and the page content. This is particularly valuable if your menu background
        /// and page background colors are similar in color. You can always set this to 0.
        /// </summary>
        public Thickness PaneBorderThickness
        {
            get { return (Thickness)GetValue(PaneBorderThicknessProperty); }
            set { SetValue(PaneBorderThicknessProperty, value); }
        }
        public static readonly DependencyProperty PaneBorderThicknessProperty =
            DependencyProperty.Register(nameof(PaneBorderThickness), typeof(Thickness),
                typeof(HamburgerMenu), new PropertyMetadata(new Thickness(0, 0, 1, 0), (d, e) =>
                {
                    WriteDebug(nameof(PaneBorderThickness), e);
                    (d as HamburgerMenu).PaneBorderThicknessChanged?.Invoke(d, e.ToChangedEventArgs<Thickness>());
                    (d as HamburgerMenu).InternalPaneBorderThicknessChanged(e.ToChangedEventArgs<Thickness>());
                }));
        public event EventHandler<ChangedEventArgs<Thickness>> PaneBorderThicknessChanged;
        partial void InternalPaneBorderThicknessChanged(ChangedEventArgs<Thickness> e);


        /// <summary>
        /// TODO: deprecate this
        /// </summary>
        public UIElement HeaderContent
        {
            get { return (UIElement)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register(nameof(HeaderContent), typeof(UIElement),
         typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    WriteDebug(nameof(HeaderContent), e);
                    (d as HamburgerMenu).HeaderContentChanged?.Invoke(d, e.ToChangedEventArgs<UIElement>());
                    (d as HamburgerMenu).InternalHeaderContentChanged(e.ToChangedEventArgs<UIElement>());
                }));
        public event EventHandler<ChangedEventArgs<UIElement>> HeaderContentChanged;
        partial void InternalHeaderContentChanged(ChangedEventArgs<UIElement> e);


        /// <summary>
        /// The Hamburger Menu is made up of two parts, the navigation Pane that holds the hamburger buttons
        /// and the content area that holds the frame and subsequent pages. This property determines if the 
        /// navitation Pane is visible or not visible. The effect varies depending on HamburgerMenu.DisplayMode.
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool),
                typeof(HamburgerMenu), new PropertyMetadata(false, (d, e) =>
                {
                    WriteDebug(nameof(IsOpen), e);
                    (d as HamburgerMenu).IsOpenChanged?.Invoke(d, e.ToChangedEventArgs<bool>());
                    (d as HamburgerMenu).InternalIsOpenChanged(e.ToChangedEventArgs<bool>());
                }));
        public event EventHandler<ChangedEventArgs<bool>> IsOpenChanged;
        partial void InternalIsOpenChanged(ChangedEventArgs<bool> e);


        /// <summary>
        /// SecondaryButtons are the button at the top of the HamburgerMenu
        /// </summary>
        public ObservableCollection<HamburgerButtonInfo> PrimaryButtons
        {
            get { return (ObservableCollection<HamburgerButtonInfo>)base.GetValue(PrimaryButtonsProperty); }
            set { SetValue(PrimaryButtonsProperty, value); }
        }
        public static readonly DependencyProperty PrimaryButtonsProperty =
            DependencyProperty.Register(nameof(PrimaryButtons), typeof(ObservableCollection<HamburgerButtonInfo>),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    WriteDebug(nameof(PrimaryButtons), e);
                }));

        /// <summary>
        /// SecondaryButtons are the button at the bottom of the HamburgerMenu
        /// </summary>
        public ObservableCollection<HamburgerButtonInfo> SecondaryButtons
        {
            get { return (ObservableCollection<HamburgerButtonInfo>)base.GetValue(SecondaryButtonsProperty); }
            set { SetValue(SecondaryButtonsProperty, value); }
        }
        public static readonly DependencyProperty SecondaryButtonsProperty =
            DependencyProperty.Register(nameof(SecondaryButtons), typeof(ObservableCollection<HamburgerButtonInfo>),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    WriteDebug(nameof(SecondaryButtons), e);
                }));

        // debug write change
        private static void WriteDebug(string v, DependencyPropertyChangedEventArgs e)
        {
            DebugWrite($"OldValue: {e.OldValue} NewValue: {e.NewValue}", caller: v);
        }
    }
}
