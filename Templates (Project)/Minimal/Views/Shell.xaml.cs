using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Template10.Views
{
    public sealed partial class Shell : Page
    {
        public Shell(NavigationService navigationService)
        {
            this.InitializeComponent();

            NavigationService = navigationService;
            this.ShellSplitView.Content = navigationService.FrameFacade.Frame;
            navigationService.FrameFacade.Navigated += (s, e) => UpdateButtons(e);
            this.Loaded += (s, e) => UpdateButtons();

            ShellSplitView.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (s, e) =>
            {
                PaneWidth = !ShellSplitView.IsPaneOpen ? ShellSplitView.CompactPaneLength : ShellSplitView.OpenPaneLength;
            });
            PaneWidth = !ShellSplitView.IsPaneOpen ? ShellSplitView.CompactPaneLength : ShellSplitView.OpenPaneLength;
        }

        Mvvm.DelegateCommand _hamburgerCommand;
        public Mvvm.DelegateCommand HamburgerCommand { get { return _hamburgerCommand ?? (_hamburgerCommand = new Mvvm.DelegateCommand(ExecuteHamburger)); } }
        private void ExecuteHamburger()
        {
            ShellSplitView.IsPaneOpen = !ShellSplitView.IsPaneOpen;
        }

        Mvvm.DelegateCommand<CommandInfo> _navCommand;
        public Mvvm.DelegateCommand<CommandInfo> NavCommand { get { return _navCommand ?? (_navCommand = new Mvvm.DelegateCommand<CommandInfo>(ExecuteNav)); } }
        private void ExecuteNav(CommandInfo commandInfo)
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
                try
                {
                    if (commandInfo.ClearHistory)
                        NavigationService.ClearHistory();
                }
                catch { }
            }
            var type = commandInfo.PageType;
        }

        NavigationService NavigationService { get; set; }

        private void UpdateButtons(NavigatedEventArgs e) { UpdateButtons(e.PageType); }
        void UpdateButtons() { UpdateButtons(NavigationService.FrameFacade.CurrentPageType); }
        void UpdateButtons(Type type)
        {
            // update radiobuttons after frame navigates
            foreach (var radioButton in Template10.Common.XamlHelper.AllChildren<RadioButton>(this))
            {
                var target = radioButton.CommandParameter as CommandInfo;
                if (target == null)
                    continue;
                radioButton.IsChecked = target.PageType.Equals(type);
            }
            this.ShellSplitView.IsPaneOpen = false;
        }

        // menu
        DelegateCommand _menuCommand;
        public DelegateCommand MenuCommand { get { return _menuCommand ?? (_menuCommand = new DelegateCommand(ExecuteMenu)); } }
        private void ExecuteMenu()
        {
            this.ShellSplitView.IsPaneOpen = !this.ShellSplitView.IsPaneOpen;
        }

        public double PaneWidth
        {
            get { return (double)GetValue(PaneWidthProperty); }
            set { SetValue(PaneWidthProperty, value); }
        }
        public static readonly DependencyProperty PaneWidthProperty =
            DependencyProperty.Register("PaneWidth", typeof(double),
                typeof(Shell), new PropertyMetadata(220));
    }

    public class CommandInfo
    {
        public Type PageType { get; set; }
        public object PageParameter { get; set; }
        public bool ClearHistory { get; set; } = false;
    }
}
