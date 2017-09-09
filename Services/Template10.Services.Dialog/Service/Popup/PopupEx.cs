using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Controls.Dialog
{
    public class PopupEx
    {
        private Windows.UI.Xaml.Controls.Primitives.Popup _popup;
        private FrameworkElement _child;
        private ContentPresenter _presenter;

        public bool IsShowing { get; private set; } = false;

        public Visibility CloseButtonVisibility { get; set; } = Visibility.Visible;

        public TransitionCollection TransitionCollection { get; set; }

        public DataTemplate Template { get; set; }

        public Brush BackgroundBrush { get; set; } = new SolidColorBrush
        {
            Color = Windows.UI.Colors.Black,
            Opacity = .5d,
        };

        public void Show(object content)
        {
            var style = new Style
            {
                TargetType = typeof(Grid),
            };
            style.Setters.Add(new Setter
            {
                Property = Grid.BackgroundProperty,
                Value = BackgroundBrush,
            });
            Show(Template, style);
        }

        public void Show(object content, Style gridStyle)
        {
            _child = new Grid
            {
                Width = Window.Current.Bounds.Width,
                Height = Window.Current.Bounds.Height,
                Style = gridStyle,
            };
            _presenter = new ContentPresenter
            {
                Width = Window.Current.Bounds.Width,
                Height = Window.Current.Bounds.Height,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                ContentTemplate = Template,
                Content = content,
            };
            if (TransitionCollection != null)
            {
                _presenter.ContentTransitions = TransitionCollection;
            }
            (_child as Grid).Children.Add(_presenter);
            var _focus = new TextBox
            {
                PreventKeyboardDisplayOnProgrammaticFocus = true,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                AllowFocusWhenDisabled = true,
                Opacity = 1.001d,
                IsEnabled = false,
                Height = 1d,
                Width = 1d,
            };
            (_child as Grid).Children.Add(_focus);
            _popup = new Windows.UI.Xaml.Controls.Primitives.Popup
            {
                Child = _child,
            };
            _popup.Opened += (s, e) =>
            {
                _focus.LosingFocus += (s1, e1) => e1.Handled = true;
                _focus.Focus(FocusState.Programmatic);
            };
            Window.Current.SizeChanged += Resize;
            _popup.IsOpen = IsShowing = true;
        }

        private void Resize(object sender, WindowSizeChangedEventArgs e)
        {
            _presenter.Width = _child.Width = Window.Current.Bounds.Width;
            _presenter.Width = _child.Height = Window.Current.Bounds.Height;
        }

        public void Hide()
        {
            _popup.IsOpen = IsShowing = false;
            Window.Current.SizeChanged -= Resize;
        }
    }
}
