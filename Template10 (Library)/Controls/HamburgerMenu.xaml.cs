using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Template10.Common;
using Template10.Services.KeyboardService;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    [ContentProperty(Name = nameof(PrimaryButtons))]
    public sealed partial class HamburgerMenu : UserControl
    {
        const int squareWidth = 48;
        const int squareHeight = 48;
        delegate void PropertyChangeHandlerDelegate(DependencyPropertyChangedEventArgs e);

        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"HamburgerMenu.{caller}");

        #endregion

        public HamburgerMenu()
        {
            DebugWrite();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                InitializeComponent();
            }
            else
            {
                // hamburger menu property changes
                PropertyChangedHandlers.Add(nameof(IsFullScreen), e => FullScreenPropertyChanged((bool?)e.OldValue, (bool?)e.NewValue));
                PropertyChangedHandlers.Add(nameof(Selected), e => SelectedPropertyChanged(e.OldValue as HamburgerButtonInfo, e.NewValue as HamburgerButtonInfo));
                PropertyChangedHandlers.Add(nameof(DisplayMode), e => DisplayModePropertyChanged((SplitViewDisplayMode)e.OldValue, (SplitViewDisplayMode)e.NewValue));
                PropertyChangedHandlers.Add(nameof(HamburgerButtonVisibility), e => HamburgerButtonVisibilityPropertyChanged((Visibility)e.NewValue));
                PropertyChangedHandlers.Add(nameof(IsOpen), e => IsOpenPropertyChanged((bool)e.OldValue, (bool)e.NewValue));
                PropertyChangedHandlers.Add(nameof(NavigationService), e => NavigationServicePropertyChanged(e.OldValue as INavigationService, e.NewValue as INavigationService));
                PropertyChangedHandlers.Add(nameof(AccentColor), e => AccentColorPropertyChanged(e.OldValue as Color?, e.NewValue as Color?));
                PropertyChangedHandlers.Add(nameof(HeaderContent), e => HeaderContentPropertyChanged(e.OldValue, e.NewValue));

                // default values;
                PrimaryButtons = new ObservableCollection<HamburgerButtonInfo>();
                SecondaryButtons = new ObservableCollection<HamburgerButtonInfo>();

                // calling this now, let's handlers wire up before styles apply
                InitializeComponent();

                // control event handlers
                Loaded += HamburgerMenu_Loaded;
                LayoutUpdated += HamburgerMenu_LayoutUpdated;

                // xbox controller menu button support
                KeyboardService.Instance.AfterMenuGesture += () =>
                {
                    HamburgerCommand.Execute();
                    HamburgerButton.Focus(FocusState.Programmatic);
                };

                GotFocus += (s, e) =>
                {
                    var element = FocusManager.GetFocusedElement() as FrameworkElement;
                    var name = element?.Name ?? "no-name";
                    var content = (((element as ContentControl)?.Content as StackPanel)?.Children[0] as SymbolIcon)?.Symbol.ToString();
                    if (content == null) content = (element as ContentControl)?.Content?.ToString() ?? "no-content";
                    var value = $"{element?.ToString() ?? "null"} {name} {content}";
                    DebugWrite(value, caller: "GotFocus");
                };
            }

        }

        void HamburgerMenu_Loaded(object sender, RoutedEventArgs args)
        {
            DebugWrite();

            // non-custom property changes
            ShellSplitView.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (d, e) => SplitViewIsPaneOpenChanged(e));
            ShellSplitView.RegisterPropertyChangedCallback(SplitView.DisplayModeProperty, (d, e) => SplitViewDisplayModeChanged(e));
            RegisterPropertyChangedCallback(RequestedThemeProperty, (d, e) => RefreshStyles(RequestedTheme));

            // keyboard navigation
            HamburgerButton.KeyDown += HamburgerMenu_KeyDown;
            PrimaryButtonContainer.KeyDown += HamburgerMenu_KeyDown;
            SecondaryButtonContainer.KeyDown += HamburgerMenu_KeyDown;

            // initial styles
            UpdateHamburgerButtonGridWidth();
            RefreshStyles(RequestedTheme);
            UpdatePaneMargin();
        }

        void HamburgerMenu_LayoutUpdated(object sender, object e)
        {
            DebugWrite();

            LayoutUpdated -= HamburgerMenu_LayoutUpdated;
            UpdateFullScreen();
        }

        #region property changed handlers

        private void HeaderContentPropertyChanged(object oldValue, object newValue) => UpdatePaneMargin();

        void SplitViewDisplayModeChanged(DependencyProperty dp) => DisplayMode = ShellSplitView.DisplayMode;

        void SplitViewIsPaneOpenChanged(DependencyProperty dp)
        {
            // this can occur if the user resizes before it loads
            if (_SecondaryButtonStackPanel == null)
            {
                return;
            }

            UpdateSecondaryButtonOrientation();
            UpdateHamburgerButtonGridWidth();
            RaisePaneOpenedClosedEvents();

            // this will keep the two properties in sync
            if (IsOpen != ShellSplitView.IsPaneOpen)
                IsOpen = ShellSplitView.IsPaneOpen;
        }

        void AccentColorPropertyChanged(Color? previous, Color? value) => RefreshStyles(value);

        void FullScreenPropertyChanged(bool? previous, bool? value) => UpdateFullScreen(value);

        void IsOpenPropertyChanged(bool previous, bool value)
        {
            // this will keep the two properties in sync
            UpdateIsPaneOpen(value);
            UpdateHamburgerButtonGridWidth();
        }

        void DisplayModePropertyChanged(SplitViewDisplayMode previous, SplitViewDisplayMode value)
        {
            // this will keep the two properties in sync
            if (ShellSplitView.DisplayMode != value)
            {
                ShellSplitView.DisplayMode = value;
            }
            HamburgerButtonGridWidth = (value == SplitViewDisplayMode.CompactInline) ? PaneWidth : squareWidth;
        }

        void HamburgerButtonVisibilityPropertyChanged(Visibility value)
        {
            HamburgerButton.Visibility = value;
            UpdatePaneMargin();
        }

        async void SelectedPropertyChanged(HamburgerButtonInfo previous, HamburgerButtonInfo value)
        {
            if ((value?.Equals(previous) ?? false))
            {
                value.IsChecked = (value.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);
            }

            SelectedChanged?.Invoke(this, new ChangedEventArgs<HamburgerButtonInfo>(previous, value));

            try
            {
                await UpdateSelectedAsync(previous, value);
            }
            catch (Exception ex)
            {
                DebugWrite($"Catch Ex.Message: {ex.Message}", caller: "SelectedPropertyChanged");
            }
        }

        void NavigationServicePropertyChanged(INavigationService previous, INavigationService value)
        {
            ShellSplitView.Content = value.FrameFacade.Frame;

            // If splash screen then continue showing until navigated once
            if (value.FrameFacade.BackStackDepth == 0
                && value.Frame.Content != null
                && BootStrapper.Current.SplashFactory != null
                && BootStrapper.Current.OriginalActivatedArgs.PreviousExecutionState != Windows.ApplicationModel.Activation.ApplicationExecutionState.Terminated)
            {
                var once = false;
                IsFullScreen = true;
                value.FrameFacade.Navigated += (s, e) =>
                {
                    if (!once)
                    {
                        once = true;
                        IsFullScreen = false;
                    }
                };
            }

            UpdateFullScreen();

            value.AfterRestoreSavedNavigation += (s, e) => HighlightCorrectButton(NavigationService.CurrentPageType, NavigationService.CurrentPageParam);
            value.FrameFacade.Navigated += (s, e) => HighlightCorrectButton(e.PageType, e.Parameter);
        }

        void IsFullScreenPropertyChanged(bool previous, bool value) => UpdateFullScreen(value);

        #endregion

        #region update methods

        internal void HighlightCorrectButton(Type pageType, object pageParam)
        {
            DebugWrite($"PageType: {pageType} PageParam: {pageParam}");

            // match type only
            var buttons = LoadedNavButtons.Where(x => Equals(x.HamburgerButtonInfo.PageType, pageType));

            // serialize parameter for matching
            if (pageParam == null)
            {
                pageParam = NavigationService.CurrentPageParam;
            }
            else if (pageParam.ToString().StartsWith("{"))
            {
                try
                {
                    pageParam = NavigationService.FrameFacade.SerializationService.Deserialize(pageParam.ToString());
                }
                catch { }
            }

            // add parameter match
            buttons = buttons.Where(x => Equals(x.HamburgerButtonInfo.PageParameter, null) || Equals(x.HamburgerButtonInfo.PageParameter, pageParam));
            var button = buttons.Select(x => x.HamburgerButtonInfo).FirstOrDefault();
            Selected = button;
        }

        private void UpdateIsPaneOpen(bool isPaneOpen)
        {
            if (isPaneOpen)
            {
                ShellSplitView.IsPaneOpen = true;
            }
            else
            {
                // collapse the window
                if (ShellSplitView.DisplayMode == SplitViewDisplayMode.Overlay && ShellSplitView.IsPaneOpen)
                {
                    ShellSplitView.IsPaneOpen = false;
                }
                else if (ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactOverlay && ShellSplitView.IsPaneOpen)
                {
                    ShellSplitView.IsPaneOpen = false;
                }
                else if (ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactInline && ShellSplitView.IsPaneOpen)
                {
                    ShellSplitView.IsPaneOpen = false;
                }
            }
        }

        private void UpdateSecondaryButtonOrientation()
        {
            // secondary layout
            if (SecondaryButtonOrientation.Equals(Orientation.Horizontal) && ShellSplitView.IsPaneOpen)
            {
                _SecondaryButtonStackPanel.Orientation = Orientation.Horizontal;
            }
            else
            {
                _SecondaryButtonStackPanel.Orientation = Orientation.Vertical;
            }
        }

        private void RaisePaneOpenedClosedEvents()
        {
            // overall events
            if (ShellSplitView.IsPaneOpen)
            {
                PaneOpened?.Invoke(ShellSplitView, EventArgs.Empty);
            }
            else
            {
                PaneClosed?.Invoke(ShellSplitView, EventArgs.Empty);
            }
        }

        private void UpdateHamburgerButtonGridWidth()
        {
            if (ShellSplitView.IsPaneOpen)
                HamburgerButtonGridWidth = (ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactInline) ? ShellSplitView.OpenPaneLength : squareWidth;
            else
                HamburgerButtonGridWidth = squareWidth;
        }

        public void UpdatePaneMargin()
        {
            if (HamburgerButtonVisibility == Visibility.Collapsed && HeaderContent == null)
                PaneContent.Margin= new Thickness(0, 0, 0, 0);
            else
                PaneContent.Margin= new Thickness(0, squareHeight, 0, 0);
        }

        async Task UpdateSelectedAsync(HamburgerButtonInfo previous, HamburgerButtonInfo value)
        {
            DebugWrite($"OldValue: {previous}, NewValue: {value}");

            // pls. do not remove this if statement. this is the fix for #410 (click twice)
            if (previous != null)
            {
                IsOpen = (DisplayMode == SplitViewDisplayMode.CompactInline && IsOpen);
            }

            // signal previous
            if (previous != null && previous != value && previous.IsChecked.Value)
            {
                previous.IsChecked = false;
                previous.RaiseUnselected();
            }

            // navigate only when all navigation buttons have been loaded
            if (AllNavButtonsAreLoaded && value?.PageType != null)
            {
                if (await NavigationService.NavigateAsync(value.PageType, value?.PageParameter, value?.NavigationTransitionInfo))
                {
                    IsOpen = (DisplayMode == SplitViewDisplayMode.CompactInline && IsOpen);
                    if (value.ClearHistory)
                        NavigationService.ClearHistory();
                }
                else if (NavigationService.CurrentPageType == value.PageType
                     && (NavigationService.CurrentPageParam ?? string.Empty) == (value.PageParameter ?? string.Empty))
                {
                    if (value.ClearHistory)
                        NavigationService.ClearHistory();
                }
                else if (NavigationService.CurrentPageType == value.PageType)
                {
                    // just check it
                }
                else
                {
                    return;
                }
            }

            // that's it if null
            if (value == null)
            {
                return;
            }
            else
            {
                value.IsChecked = (value.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);
                if (previous != value)
                {
                    value.RaiseSelected();
                }
            }
        }

        /// <summary>
        /// When IsFullScreen is true, the content is displayed on top of the SplitView and the SplitView is
        /// not visible. Even as the user navigates (if possible) the SplitView remains hidden until
        /// IsFullScreen is set to false.
        /// </summary>
        /// <remarks>
        /// The original intent for this property was to allow the splash screen to be visible while the
        /// remaining content loaded duing app start. In Minimal (Shell), this is still used for this purpose,
        /// but many developers also leverage this property to view media full screen and similar use cases.
        /// </remarks>
        void UpdateFullScreen(bool? manual = null)
        {
            DebugWrite($"Manual: {manual}, IsFullScreen: {IsFullScreen}");

            var frame = NavigationService?.Frame;
            if (manual ?? IsFullScreen)
            {
                HamburgerButton.Opacity = 0;
                ShellSplitView.IsHitTestVisible = ShellSplitView.IsEnabled = false;
                AutomationProperties.SetAccessibilityView(ShellSplitView, Windows.UI.Xaml.Automation.Peers.AccessibilityView.Raw);
                ShellSplitView.Content = null;
                if (!RootGrid.Children.Contains(frame) && frame != null)
                {
                    RootGrid.Children.Add(frame);
                }
            }
            else
            {
                HamburgerButton.Opacity = 1;
                ShellSplitView.IsHitTestVisible = ShellSplitView.IsEnabled = true;
                AutomationProperties.SetAccessibilityView(ShellSplitView, Windows.UI.Xaml.Automation.Peers.AccessibilityView.Control);
                if (RootGrid.Children.Contains(frame) && frame != null)
                {
                    RootGrid.Children.Remove(frame);
                }
                ShellSplitView.Content = frame;
            }
        }

        #endregion

        StackPanel _SecondaryButtonStackPanel;
        void SecondaryButtonStackPanel_Loaded(object sender, RoutedEventArgs e) => _SecondaryButtonStackPanel = sender as StackPanel;

        #region Nav Buttons

        #region commands

        Mvvm.DelegateCommand _hamburgerCommand;
        internal Mvvm.DelegateCommand HamburgerCommand => _hamburgerCommand ?? (_hamburgerCommand = new Mvvm.DelegateCommand(ExecuteHamburger));
        void ExecuteHamburger()
        {
            DebugWrite();

            IsOpen = !IsOpen;
        }

        Mvvm.DelegateCommand<HamburgerButtonInfo> _navCommand;
        public Mvvm.DelegateCommand<HamburgerButtonInfo> NavCommand => _navCommand ?? (_navCommand = new Mvvm.DelegateCommand<HamburgerButtonInfo>(ExecuteNav));
        void ExecuteNav(HamburgerButtonInfo commandInfo)
        {
            DebugWrite($"HamburgerButtonInfo: {commandInfo}");

            if (commandInfo == null)
            {
                throw new NullReferenceException("CommandParameter is not set");
            }

            if (commandInfo.PageType != null)
            {
                Selected = commandInfo;
            }
            else
            {
                ExecuteICommand(commandInfo);
                commandInfo.RaiseTapped(new RoutedEventArgs());
            }
        }

        #endregion

        int NavButtonCount => PrimaryButtons.Count + SecondaryButtons.Count;
        bool AllNavButtonsAreLoaded => LoadedNavButtons.Count >= NavButtonCount;
        readonly List<InfoElement> LoadedNavButtons = new List<InfoElement>();

        public class InfoElement
        {
            public InfoElement(object sender)
            {
                FrameworkElement = sender as FrameworkElement;
                HamburgerButtonInfo = FrameworkElement?.DataContext as HamburgerButtonInfo;
            }
            public FrameworkElement FrameworkElement { get; }
            public HamburgerButtonInfo HamburgerButtonInfo { get; }
        }

        void NavButton_Loaded(object sender, RoutedEventArgs e)
        {
            DebugWrite();

            var button = new InfoElement(sender);
            if (!LoadedNavButtons.Any(x => x.FrameworkElement == button.FrameworkElement))
            {
                LoadedNavButtons.Add(button);
                if (AllNavButtonsAreLoaded)
                {
                    HighlightCorrectButton(NavigationService.CurrentPageType, NavigationService.CurrentPageParam);
                }
            }
        }

        //void NavButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        //{
        //    DebugWrite();

        //    var button = new InfoElement(sender);
        //    ExecuteICommand(button.HamburgerButtonInfo);
        //    button.HamburgerButtonInfo.RaiseTapped(e);
        //    e.Handled = true;
        //}

        void ExecuteICommand(HamburgerButtonInfo info)
        {
            ICommand command = info.Command;
            if (command != null)
            {
                var commandParameter = info.CommandParameter;
                if (command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            }
        }

        void NavButton_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            DebugWrite();

            var button = new InfoElement(sender);
            button.HamburgerButtonInfo.RaiseRightTapped(e);
            e.Handled = true;
        }

        void NavButton_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            DebugWrite();

            var button = new InfoElement(sender);
            button.HamburgerButtonInfo.RaiseHolding(e);
            e.Handled = true;
        }

        void NavButtonChecked(object sender, RoutedEventArgs e)
        {
            DebugWrite();

            var button = new InfoElement(sender);
            if (button.HamburgerButtonInfo.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle)
            {
                button.HamburgerButtonInfo.IsChecked = (button.HamburgerButtonInfo.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle);
                if (button.HamburgerButtonInfo.IsChecked ?? true) Selected = button.HamburgerButtonInfo;
                if (button.HamburgerButtonInfo.IsChecked ?? true) button.HamburgerButtonInfo.RaiseChecked(e);
                button.FrameworkElement.IsHitTestVisible = !button.HamburgerButtonInfo.IsChecked ?? false;
            }
        }

        void NavButtonUnchecked(object sender, RoutedEventArgs e)
        {
            DebugWrite();

            var button = new InfoElement(sender);
            if (button.HamburgerButtonInfo.ButtonType == HamburgerButtonInfo.ButtonTypes.Toggle)
            {
                button.HamburgerButtonInfo.RaiseUnchecked(e);
                button.FrameworkElement.IsHitTestVisible = true;
            }
        }

        private void NavButton_VisualStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            var button = new InfoElement(e.Control);
            switch (e.NewState.Name)
            {
                case "Normal":
                case "PointerOver":
                case "Pressed":
                case "Disabled":
                case "Checked":
                case "CheckedPointerOver":
                case "CheckedPressed":
                case "CheckedDisabled":
                case "Indeterminate":
                case "IndeterminatePointerOver":
                case "IndeterminatePressed":
                case "IndeterminateDisabled":
                default:
                    break;
            }
        }

        #endregion

        #region  Touch gesture to OpenClose

        void PaneContent_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DebugWrite($"OpenCloseMode {OpenCloseMode}");

            var button = new InfoElement(e.OriginalSource);
            if (button.HamburgerButtonInfo?.IsChecked ?? false) return;

            switch (OpenCloseMode)
            {
                case OpenCloseModes.Auto:
                case OpenCloseModes.Tap:
                    HamburgerCommand.Execute(null);
                    break;
            }
        }

        void PaneContent_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            DebugWrite($"OpenCloseMode {OpenCloseMode}");

            if (e.PointerDeviceType == PointerDeviceType.Mouse) return;
            if (e.PointerDeviceType == PointerDeviceType.Pen) return;
            switch (OpenCloseMode)
            {
                case OpenCloseModes.None:
                case OpenCloseModes.Tap:
                    return;
            }

            var threshold = 24;
            var delta = e.Cumulative.Translation.X;
            if (delta < -threshold) IsOpen = false;
            else if (delta > threshold) IsOpen = true;
        }

        #endregion

        // handle keyboard navigation (tabs and gamepad)
        private void HamburgerMenu_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            var currentItem = FocusManager.GetFocusedElement() as FrameworkElement;
            var lastItem = LoadedNavButtons.FirstOrDefault(x => x.HamburgerButtonInfo == (SecondaryButtons.LastOrDefault(a => a != Selected) ?? PrimaryButtons.LastOrDefault(a => a != Selected)));

            var focus = new Func<FocusNavigationDirection, bool>(d =>
            {
                if (d == FocusNavigationDirection.Next)
                {
                    return FocusManager.TryMoveFocus(d);
                }
                else if (d == FocusNavigationDirection.Previous)
                {
                    return FocusManager.TryMoveFocus(d);
                }
                else
                {
                    var control = FocusManager.FindNextFocusableElement(d) as Control;
                    return control?.Focus(FocusState.Programmatic) ?? false;
                }
            });

            var escape = new Func<bool>(() =>
            {
                if (DisplayMode == SplitViewDisplayMode.CompactOverlay
                    || DisplayMode == SplitViewDisplayMode.Overlay)
                    IsOpen = false;
                if (Equals(ShellSplitView.PanePlacement, SplitViewPanePlacement.Left))
                {
                    ShellSplitView.Content.RenderTransform = new TranslateTransform { X = 48 + ShellSplitView.OpenPaneLength };
                    focus(FocusNavigationDirection.Right);
                    ShellSplitView.Content.RenderTransform = null;
                }
                else
                {
                    ShellSplitView.Content.RenderTransform = new TranslateTransform { X = -48 - ShellSplitView.OpenPaneLength };
                    focus(FocusNavigationDirection.Left);
                    ShellSplitView.Content.RenderTransform = null;
                }
                return true;
            });

            var previous = new Func<bool>(() =>
            {
                if (Equals(currentItem, HamburgerButton))
                {
                    return true;
                }
                else if (focus(FocusNavigationDirection.Previous) || focus(FocusNavigationDirection.Up))
                {
                    return true;
                }
                else
                {
                    return escape();
                }
            });

            var next = new Func<bool>(() =>
            {
                if (Equals(currentItem, HamburgerButton))
                {
                    return focus(FocusNavigationDirection.Down);
                }
                else if (focus(FocusNavigationDirection.Next) || focus(FocusNavigationDirection.Down))
                {
                    return true;
                }
                else
                {
                    return escape();
                }
            });

            if (IsFullScreen)
            {
                return;
            }

            switch (e.Key)
            {
                case VirtualKey.Up:
                case VirtualKey.GamepadDPadUp:

                    if (!(e.Handled = previous())) Debugger.Break();
                    break;

                case VirtualKey.Down:
                case VirtualKey.GamepadDPadDown:

                    if (!(e.Handled = next())) Debugger.Break();
                    break;

                case VirtualKey.Right:
                case VirtualKey.GamepadDPadRight:
                    if (SecondaryButtonContainer.Items.Contains(currentItem?.DataContext)
                        && SecondaryButtonOrientation == Orientation.Horizontal)
                    {
                        if (Equals(lastItem.FrameworkElement, currentItem))
                        {
                            if (!(e.Handled = escape())) Debugger.Break();
                        }
                        else
                        {
                            if (!(e.Handled = next())) Debugger.Break();
                        }
                    }
                    else
                    {
                        if (!(e.Handled = escape())) Debugger.Break();
                    }
                    break;

                case VirtualKey.Left:
                case VirtualKey.GamepadDPadLeft:

                    if (SecondaryButtonContainer.Items.Contains(currentItem?.DataContext)
                       && SecondaryButtonOrientation == Orientation.Horizontal)
                    {
                        if (Equals(lastItem.FrameworkElement, currentItem))
                        {
                            if (!(e.Handled = escape())) Debugger.Break();
                        }
                        else
                        {
                            if (!(e.Handled = previous())) Debugger.Break();
                        }
                    }
                    else
                    {
                        if (!(e.Handled = escape())) Debugger.Break();
                    }
                    break;

                case VirtualKey.Space:
                case VirtualKey.Enter:
                case VirtualKey.GamepadA:

                    var info = new InfoElement(currentItem);
                    NavCommand.Execute(info?.HamburgerButtonInfo);
                    break;

                case VirtualKey.Escape:
                case VirtualKey.GamepadB:

                    if (!(e.Handled = escape())) Debugger.Break();
                    break;
            }
        }
    }
}
