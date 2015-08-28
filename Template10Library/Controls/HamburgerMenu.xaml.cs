using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Template10.Services.KeyboardService;
using Template10.Services.NavigationService;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-HamburgerMenu
    [ContentProperty(Name = nameof(PrimaryButtons))]
    public sealed partial class HamburgerMenu : UserControl, INotifyPropertyChanged
    {
        public HamburgerMenu()
        {
            this.InitializeComponent();
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }
            new KeyboardService().AfterWindowZGesture = () => { HamburgerCommand.Execute(null); };

        }

        void UpdateButtons(NavigatedEventArgs e) { UpdateButtons(e.PageType); }
        void UpdateButtons() { UpdateButtons(NavigationService.Frame.Content?.GetType()); }
        void UpdateButtons(Type type)
        {
            var info = _navButtons.FirstOrDefault(x => x.Value.PageType.Equals(type));
            Selected = info.Value;
        }

        Mvvm.DelegateCommand _hamburgerCommand;
        internal Mvvm.DelegateCommand HamburgerCommand { get { return _hamburgerCommand ?? (_hamburgerCommand = new Mvvm.DelegateCommand(ExecuteHamburger)); } }
        void ExecuteHamburger() { IsOpen = !IsOpen; }

        Mvvm.DelegateCommand<NavigationButtonInfo> _navCommand;
        public Mvvm.DelegateCommand<NavigationButtonInfo> NavCommand { get { return _navCommand ?? (_navCommand = new Mvvm.DelegateCommand<NavigationButtonInfo>(ExecuteNav)); } }
        void ExecuteNav(NavigationButtonInfo commandInfo)
        {
            if (commandInfo == null)
                throw new NullReferenceException("CommandParameter is not set");
            try
            {
                // navigate only to new pages
                if (commandInfo.PageType != null && NavigationService.CurrentPageType != commandInfo.PageType)
                    NavigationService.Navigate(commandInfo.PageType, commandInfo.PageParameter);
                Selected = commandInfo;
            }
            finally
            {
                if (commandInfo.ClearHistory)
                    NavigationService.ClearHistory();
            }
        }

        #region VisualStateValues

        public double VisualStateNarrowMinWidth
        {
            get { return VisualStateNarrowTrigger.MinWindowWidth; }
            set { SetValue(VisualStateNarrowMinWidthProperty, VisualStateNarrowTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateNarrowMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNarrowMinWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).VisualStateNarrowMinWidth = (double)e.NewValue; }));

        public double VisualStateNormalMinWidth
        {
            get { return VisualStateNormalTrigger.MinWindowWidth; }
            set { SetValue(VisualStateNormalMinWidthProperty, VisualStateNormalTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateNormalMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNormalMinWidth), typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).VisualStateNormalMinWidth = (double)e.NewValue; }));

        #endregion

        #region Style

        public SolidColorBrush HamburgerBackground
        {
            get { return (SolidColorBrush)HamburgerBackgroundBrush; }
            set
            {
                SetValue(HamburgerBackgroundProperty, HamburgerBackgroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HamburgerBackground)));
            }
        }
        public static readonly DependencyProperty HamburgerBackgroundProperty =
            DependencyProperty.Register(nameof(HamburgerBackground), typeof(SolidColorBrush),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).HamburgerBackground = (SolidColorBrush)e.NewValue; }));

        public SolidColorBrush HamburgerForeground
        {
            get { return HamburgerForegroundBrush; }
            set
            {
                SetValue(HamburgerForegroundProperty, HamburgerForegroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HamburgerForeground)));
            }
        }
        public static readonly DependencyProperty HamburgerForegroundProperty =
              DependencyProperty.Register(nameof(HamburgerForeground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).HamburgerForeground = (SolidColorBrush)e.NewValue; }));

        public SolidColorBrush NavAreaBackground
        {
            get { return NavAreaBackgroundBrush; }
            set
            {
                SetValue(NavAreaBackgroundProperty, NavAreaBackgroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HamburgerForeground)));
            }
        }
        public static readonly DependencyProperty NavAreaBackgroundProperty =
              DependencyProperty.Register(nameof(NavAreaBackground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).NavAreaBackground = (SolidColorBrush)e.NewValue; }));

        public SolidColorBrush NavButtonBackground
        {
            get { return (SolidColorBrush)NavButtonBackgroundBrush; }
            set
            {
                SetValue(NavButtonBackgroundProperty, NavButtonBackgroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavButtonBackground)));
            }
        }
        public static readonly DependencyProperty NavButtonBackgroundProperty =
            DependencyProperty.Register(nameof(NavButtonBackground), typeof(SolidColorBrush),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).NavButtonBackground = (SolidColorBrush)e.NewValue; }));

        public SolidColorBrush NavButtonForeground
        {
            get { return NavButtonForegroundBrush; }
            set
            {
                SetValue(NavButtonForegroundProperty, NavButtonForegroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HamburgerForeground)));
            }
        }
        public static readonly DependencyProperty NavButtonForegroundProperty =
              DependencyProperty.Register(nameof(NavButtonForeground), typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).NavButtonForeground = (SolidColorBrush)e.NewValue; }));

        public SolidColorBrush SecondarySeparator
        {
            get { return SecondaryBorderBrush; }
            set
            {
                SetValue(SecondarySeparatorProperty, SecondaryBorderBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HamburgerForeground)));
            }
        }
        public static readonly DependencyProperty SecondarySeparatorProperty =
              DependencyProperty.Register("SecondarySeparator", typeof(SolidColorBrush),
                  typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) => { (d as HamburgerMenu).SecondarySeparator = (SolidColorBrush)e.NewValue; }));

        #endregion

        public NavigationButtonInfo Selected
        {
            get
            {
                // do nothing if doesn't exist
                if (!_navButtons.Any(x => x.Key.IsChecked.Value))
                {
                    if (GetValue(SelectedProperty) != null)
                        SetValue(SelectedProperty, null);
                    return null;
                }

                // get value && ensure dp is also correct
                var value = _navButtons.First(x => x.Key.IsChecked.Value).Value;
                if (GetValue(SelectedProperty) != value)
                    SetValue(SelectedProperty, value);
                return value;
            }
            set
            {
                // clear existing
                foreach (var button in _navButtons)
                {
                    button.Key.IsChecked = false;
                    button.Key.IsEnabled = true;
                }

                // don't continue if none 
                if (!_navButtons.Any(x => x.Value.Equals(value)))
                    return;

                // collapse the window
                if (ShellSplitView.DisplayMode == SplitViewDisplayMode.Overlay && ShellSplitView.IsPaneOpen)
                    ShellSplitView.IsPaneOpen = false;
                else if (ShellSplitView.DisplayMode == SplitViewDisplayMode.CompactOverlay && ShellSplitView.IsPaneOpen)
                    ShellSplitView.IsPaneOpen = false;

                // setup new value
                var navButton = _navButtons.First(x => x.Value.Equals(value));
                navButton.Key.IsChecked = true;
                navButton.Key.IsEnabled = false;

                // ensure dp is correct (if diff)
                if (GetValue(SelectedProperty) != value)
                    SetValue(SelectedProperty, value);
            }
        }
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(NavigationButtonInfo),
                typeof(HamburgerMenu), new PropertyMetadata(null, (d, e) =>
                {
                    (d as HamburgerMenu).Selected = (NavigationButtonInfo)e.NewValue;
                }));

        public bool IsOpen
        {
            get
            {
                var open = ShellSplitView.IsPaneOpen;
                if (open != (bool)GetValue(IsOpenProperty))
                    SetValue(IsOpenProperty, open);
                return open;
            }
            set
            {
                var open = ShellSplitView.IsPaneOpen;
                if (open == value)
                    return;
                ShellSplitView.IsPaneOpen = value;
                SetValue(IsOpenProperty, value);
            }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool),
                typeof(HamburgerMenu), new PropertyMetadata(false,
                    (d, e) => { (d as HamburgerMenu).IsOpen = (bool)e.NewValue; }));

        public ObservableCollection<NavigationButtonInfo> PrimaryButtons
        {
            get
            {
                var PrimaryButtons = (ObservableCollection<NavigationButtonInfo>)base.GetValue(PrimaryButtonsProperty);
                if (PrimaryButtons == null)
                    base.SetValue(PrimaryButtonsProperty, PrimaryButtons = new ObservableCollection<NavigationButtonInfo>());
                return PrimaryButtons;
            }
            set { SetValue(PrimaryButtonsProperty, value); }
        }
        public static readonly DependencyProperty PrimaryButtonsProperty =
            DependencyProperty.Register("PrimaryButtons", typeof(ObservableCollection<NavigationButtonInfo>),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        private NavigationService _navigationService;
        public NavigationService NavigationService
        {
            get { return _navigationService; }
            set
            {
                _navigationService = value;
                if (NavigationService.Frame.BackStackDepth > 0)
                {
                    // display content inside the splitview
                    ShellSplitView.Content = NavigationService.Frame;
                }
                else
                {
                    // display content without splitview (splash scenario)
                    Action revert = () =>
                    {
                        RootGrid.Children.Remove(NavigationService.Frame);
                        ShellSplitView.Content = NavigationService.Frame;
                    };
                    NavigationService.AfterRestoreSavedNavigation += (s, e) => revert();
                    NavigationService.FrameFacade.Navigated += (s, e) => revert();
                    RootGrid.Children.Add(NavigationService.Frame);
                }
                NavigationService.FrameFacade.Navigated += (s, e) => UpdateButtons(e);
                NavigationService.AfterRestoreSavedNavigation += (s, e) => UpdateButtons();
                ShellSplitView.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (s, e) =>
                {
                    // update width
                    PaneWidth = !ShellSplitView.IsPaneOpen ? ShellSplitView.CompactPaneLength : ShellSplitView.OpenPaneLength;
                });
            }
        }

        public ObservableCollection<NavigationButtonInfo> SecondaryButtons
        {
            get
            {
                var SecondaryButtons = (ObservableCollection<NavigationButtonInfo>)base.GetValue(SecondaryButtonsProperty);
                if (SecondaryButtons == null)
                    base.SetValue(SecondaryButtonsProperty, SecondaryButtons = new ObservableCollection<NavigationButtonInfo>());
                return SecondaryButtons;
            }
            set { SetValue(SecondaryButtonsProperty, value); }
        }
        public static readonly DependencyProperty SecondaryButtonsProperty =
            DependencyProperty.Register("SecondaryButtons", typeof(ObservableCollection<NavigationButtonInfo>),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public double PaneWidth
        {
            get { return (double)GetValue(PaneWidthProperty); }
            set { SetValue(PaneWidthProperty, value); }
        }
        public static readonly DependencyProperty PaneWidthProperty =
            DependencyProperty.Register("PaneWidth", typeof(double),
                typeof(HamburgerMenu), new PropertyMetadata(220));

        public event PropertyChangedEventHandler PropertyChanged;

        Dictionary<RadioButton, NavigationButtonInfo> _navButtons = new Dictionary<RadioButton, NavigationButtonInfo>();
        void NavButton_Loaded(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            _navButtons.Add(radio, radio.DataContext as NavigationButtonInfo);
            UpdateButtons();
        }

        private void PaneContent_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            HamburgerCommand.Execute(null);
        }

        private void NavButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }

    [ContentProperty(Name = nameof(Content))]
    public class NavigationButtonInfo
    {
        public Type PageType { get; set; }
        public object PageParameter { get; set; }
        public bool ClearHistory { get; set; } = false;
        public UIElement Content { get; set; }
        public override string ToString()
        {
            return string.Format("{0}({1})", PageType, PageParameter);
        }
    }
}
