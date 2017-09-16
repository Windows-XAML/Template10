using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Template10.Services.Gesture;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Controls.Dialog
{
    public class PopupEx : IDisposable
    {
        private Windows.UI.Xaml.Controls.Primitives.Popup _popup;
        private FrameworkElement _child;
        private ContentPresenter _presenter;

        IGestureService _GestureService;
        CoreDispatcher _diaptcher;
        private Action ClosedCallback;

        public PopupEx()
        {
            _diaptcher = Window.Current.Dispatcher;
        }

        public PopupEx(IGestureService gestureService)
            : this()
        {
            _GestureService = gestureService;
            if (_GestureService is IGestureService2 g && g != null)
            {
                g.BackRequested2 += gesture_BackRequested2;
            }
        }

        public void Dispose()
        {
            if (_GestureService is IGestureService2 g && g != null)
            {
                g.BackRequested2 -= gesture_BackRequested2;
            }
        }

        void gesture_BackRequested2(object sender, EventArgs e)
        {
            if (IsShowing && AllowBackClose)
            {
                IsShowing = false;
            }
        }

        public bool AllowBackClose { get; set; }

        public bool IsShowing { get; private set; } = false;

        public Visibility CloseButtonVisibility { get; set; } = Visibility.Collapsed;

        public TransitionCollection TransitionCollection { get; set; }

        public DataTemplate Template { get; set; }

        public event EventHandler Closed;

        public enum Placements { Left, Right, Top, Bottom, Center, Fill }

        public Placements Placement { get; set; } = Placements.Center;

        public Brush BackgroundBrush { get; set; } = new SolidColorBrush
        {
            Color = Windows.UI.Colors.Black,
            Opacity = .5d,
        };

        public void Show(object content, Action callback = null)
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
            Show(content, style, callback);
        }

        public void Show(object content, Style gridStyle, Action callback = null)
        {
            ClosedCallback = callback;
            RunSafe(() =>
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
                    ContentTemplate = Template,
                    Content = content,
                };
                SetupPlacement(_presenter);
                if (TransitionCollection != null)
                {
                    _presenter.ContentTransitions = TransitionCollection;
                }
                (_child as Grid).Children.Add(_presenter);
                var _focus = new TextBox
                {
                    Name = "Hidden_Focus_Grabbing_TextBox",
                    RenderTransform = new ScaleTransform { ScaleX = .01, ScaleY = .01 },
                    PreventKeyboardDisplayOnProgrammaticFocus = true,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    AllowFocusWhenDisabled = true,
                    Opacity = .001d,
                    IsEnabled = false,
                    IsTabStop = false,
                    Height = 1d,
                    Width = 1d,
                };
                (_child as Grid).Children.Add(_focus);
                var close = new Button
                {
                    Content = new SymbolIcon { Symbol = Symbol.Cancel },
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(16),
                    Padding = new Thickness(16),
                    Visibility = CloseButtonVisibility,
                    RequestedTheme = ElementTheme.Dark,
                };
                close.Click += (s, e) => Hide();
                (_child as Grid).Children.Add(close);
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
            });
        }

        private void SetupPlacement(ContentPresenter presenter)
        {
            switch (Placement)
            {
                case Placements.Left:
                    presenter.HorizontalContentAlignment = HorizontalAlignment.Left;
                    presenter.VerticalContentAlignment = VerticalAlignment.Stretch;
                    _child.HorizontalAlignment = HorizontalAlignment.Left;
                    _child.VerticalAlignment = VerticalAlignment.Stretch;
                    break;
                case Placements.Right:
                    presenter.HorizontalContentAlignment = HorizontalAlignment.Right;
                    presenter.VerticalContentAlignment = VerticalAlignment.Stretch;
                    _child.HorizontalAlignment = HorizontalAlignment.Right;
                    _child.VerticalAlignment = VerticalAlignment.Stretch;
                    break;
                case Placements.Top:
                    presenter.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    presenter.VerticalContentAlignment = VerticalAlignment.Top;
                    _child.HorizontalAlignment = HorizontalAlignment.Stretch;
                    _child.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case Placements.Bottom:
                    presenter.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    presenter.VerticalContentAlignment = VerticalAlignment.Bottom;
                    _child.HorizontalAlignment = HorizontalAlignment.Stretch;
                    _child.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case Placements.Center:
                    presenter.HorizontalContentAlignment = HorizontalAlignment.Center;
                    presenter.VerticalContentAlignment = VerticalAlignment.Center;
                    _child.HorizontalAlignment = HorizontalAlignment.Center;
                    _child.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case Placements.Fill:
                    presenter.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    presenter.VerticalContentAlignment = VerticalAlignment.Stretch;
                    _child.HorizontalAlignment = HorizontalAlignment.Stretch;
                    _child.VerticalAlignment = VerticalAlignment.Stretch;
                    break;
                default:
                    break;
            }
        }

        private void Resize(object sender, WindowSizeChangedEventArgs e)
        {
            RunSafe(() =>
            {
                _presenter.Width = _child.Width = Window.Current.Bounds.Width;
                _presenter.Width = _child.Height = Window.Current.Bounds.Height;
            });
        }

        public void Hide()
        {
            RunSafe(() =>
            {
                _popup.IsOpen = IsShowing = false;
                Window.Current.SizeChanged -= Resize;
                Closed?.Invoke(this, EventArgs.Empty);
                ClosedCallback?.Invoke();
                ClosedCallback = null;
            });
        }

        async void RunSafe(Action action)
        {
            await _diaptcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
