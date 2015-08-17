using System;
using System.Collections.ObjectModel;
using Template10.Services.KeyboardService;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Template10.Controls
{
    [ContentProperty(Name = nameof(PrimaryButtons))]
    public sealed partial class HamburgerMenu : UserControl
    {
        public HamburgerMenu()
        {
            this.InitializeComponent();
            Loaded += HamburgerMenu_Loaded;

        }

        private void HamburgerMenu_Loaded(object sender, RoutedEventArgs e)
        {
            new KeyboardService().AfterWindowZGesture = () => { HamburgerCommand.Execute(null); };
        }

        void UpdateButtons(NavigatedEventArgs e) { UpdateButtons(e.PageType); }
        void UpdateButtons() { UpdateButtons(NavigationService.FrameFacade.CurrentPageType); }
        void UpdateButtons(Type type)
        {
            // update radiobuttons after frame navigates
            foreach (var radioButton in Common.XamlHelper.AllChildren<RadioButton>(this))
            {
                var target = radioButton.CommandParameter as NavigationButtonInfo;
                if (target == null)
                    continue;
                radioButton.IsChecked = target.PageType.Equals(type);
            }
            ShellSplitView.IsPaneOpen = false;
        }

        Mvvm.DelegateCommand _hamburgerCommand;
        public Mvvm.DelegateCommand HamburgerCommand { get { return _hamburgerCommand ?? (_hamburgerCommand = new Mvvm.DelegateCommand(ExecuteHamburger)); } }
        private void ExecuteHamburger()
        {
            ShellSplitView.IsPaneOpen = !ShellSplitView.IsPaneOpen;
        }

        Mvvm.DelegateCommand<NavigationButtonInfo> _navCommand;
        public Mvvm.DelegateCommand<NavigationButtonInfo> NavCommand { get { return _navCommand ?? (_navCommand = new Mvvm.DelegateCommand<NavigationButtonInfo>(ExecuteNav)); } }
        private void ExecuteNav(NavigationButtonInfo commandInfo)
        {
            if (commandInfo == null)
                throw new NullReferenceException("CommandParameter is not set");
            try
            {
                // navigate only to new pages
                if (NavigationService.CurrentPageType != null && NavigationService.CurrentPageType != commandInfo.PageType)
                    NavigationService.Navigate(commandInfo.PageType, commandInfo.PageParameter);
            }
            finally
            {
                if (commandInfo.ClearHistory)
                    NavigationService.ClearHistory();
            }
        }

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
                ShellSplitView.Content = NavigationService.FrameFacade.Frame;
                NavigationService.FrameFacade.Navigated += (s, e) => UpdateButtons(e);
                ShellSplitView.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (s, e) =>
                { PaneWidth = !ShellSplitView.IsPaneOpen ? ShellSplitView.CompactPaneLength : ShellSplitView.OpenPaneLength; });
                PaneWidth = !ShellSplitView.IsPaneOpen ? ShellSplitView.CompactPaneLength : ShellSplitView.OpenPaneLength;
                UpdateButtons();
            }
        }

        public Visibility HamburgerButtonVisibility
        {
            get { return (Visibility)GetValue(HamburgerButtonVisibilityProperty); }
            set { SetValue(HamburgerButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty HamburgerButtonVisibilityProperty =
            DependencyProperty.Register("HamburgerButtonVisibility", typeof(Visibility),
                typeof(HamburgerMenu), new PropertyMetadata(Visibility.Visible));

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
    }

    [ContentProperty(Name = nameof(Content))]
    public class NavigationButtonInfo
    {
        public Type PageType { get; set; }
        public object PageParameter { get; set; }
        public bool ClearHistory { get; set; } = false;
        public UIElement Content { get; set; }
    }
}
