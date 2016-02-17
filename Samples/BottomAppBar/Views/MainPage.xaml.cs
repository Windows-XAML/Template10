using System;
using Template10.Controls;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Messaging.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        DelegateCommand _TogglePaneCommand;
        public DelegateCommand TogglePaneCommand
           => _TogglePaneCommand ?? (_TogglePaneCommand = new DelegateCommand(TogglePaneCommandExecute, TogglePaneCommandCanExecute));
        bool TogglePaneCommandCanExecute() => true;
        void TogglePaneCommandExecute()
        {
            Shell.HamburgerMenu.IsOpen = !Shell.HamburgerMenu.IsOpen;
        }

        DelegateCommand<int> _SetPaneCommand;
        public DelegateCommand<int> SetPaneCommand
           => _SetPaneCommand ?? (_SetPaneCommand = new DelegateCommand<int>(SetPaneCommandExecute, SetPaneCommandCanExecute));
        bool SetPaneCommandCanExecute(int param) => true;
        void SetPaneCommandExecute(int param)
        {
            var h = Shell.HamburgerMenu;
            h.HamburgerButtonVisibility = Visibility.Collapsed;
            h.DisplayMode = (SplitViewDisplayMode)param;
            h.VisualStateNarrowMinWidth = -1;
            h.VisualStateNormalMinWidth = -1;
            h.VisualStateWideMinWidth = -1;
            switch (h.DisplayMode)
            {
                case SplitViewDisplayMode.Inline:
                case SplitViewDisplayMode.CompactInline:
                    h.IsOpen = true;
                    break;
                case SplitViewDisplayMode.Overlay:
                case SplitViewDisplayMode.CompactOverlay:
                    h.IsOpen = false;
                    break;
            }
        }

    }
}
