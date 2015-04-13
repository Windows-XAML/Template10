using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Views
{
    public sealed partial class Shell : Page
    {
        public Shell(Frame frame)
        {
            this.InitializeComponent();
            this.ShellSplitView.Content = frame;
            frame.Navigated += Frame_Navigated;
            this.DataContext = this;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            // update radiobuttons after frame navigates
            var type = e.SourcePageType.ToString();
            foreach (var radioButton in RadioButtonContainer.Children.OfType<RadioButton>())
            {
                var command = radioButton.CommandParameter as String;
                if (string.IsNullOrEmpty(command))
                {
                    radioButton.IsChecked = false;
                }
                else
                {
                    radioButton.IsChecked = type
                        .EndsWith(command, StringComparison.CurrentCultureIgnoreCase);
                }
            }
        }

        private void BackRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // don't let the radiobutton check
            (sender as RadioButton).IsChecked = false;

            // navigate back (if possible)
            if ((App.Current as App).NavigationService.CanGoBack)
            {
                (App.Current as App).NavigationService.GoBack();
            }
        }

        private void HamburgerRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // don't let the radiobutton check
            (sender as RadioButton).IsChecked = false;

            // toggle the splitview pane
            this.ShellSplitView.IsPaneOpen = !this.ShellSplitView.IsPaneOpen;
        }

        Mvvm.Command<string> _navCommand;
        public Mvvm.Command<string> NavCommand
        {
            get
            {
                if (_navCommand == null)
                {
                    _navCommand = new Mvvm.Command<string>(ns =>
                    {
                        // build full namespace
                        var root = App.Current.GetType().Namespace;
                        if (!ns.StartsWith(root))
                        { ns = string.Format("{0}.Views.{1}", root, ns); }

                        // attempt to find type
                        Type type = default(Type);
                        try { type = Type.GetType(ns); }
                        catch { throw new InvalidCastException(ns ?? "Not set"); }

                        // when we nav home, clear history
                        var nav = (App.Current as App).NavigationService;
                        if (type.Equals(typeof(Views.MainPage)))
                        { nav.ClearHistory(); }

                        // navigate only to new pages
                        if (nav.CurrentPageType != null && nav.CurrentPageType != type)
                        { nav.Navigate(type); }
                    });
                }
                return _navCommand;
            }
        }
    }
}
