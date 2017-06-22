using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Template10.Services.PopupService
{
    public enum PopupSize { FullScreen, ContentBased }

    public class PopupService
    {

        public Popup Create(PopupSize size, UIElement content = null)
        {
            var container = new ContentControl
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Height = Window.Current.Bounds.Height,
                Width = Window.Current.Bounds.Width,
                Content = content,
            };
            var popup = new Popup
            {
                Child = container,
                HorizontalOffset = 0,
                VerticalOffset = 0,
            };
            WindowSizeChangedEventHandler handler = (s, e) =>
            {
                if (popup.IsOpen)
                {
                    if (size == PopupSize.FullScreen)
                    {
                        container.Height = Window.Current.Bounds.Height;
                        container.Width = Window.Current.Bounds.Width;
                    }
                    else if (size == PopupSize.ContentBased)
                    {
                        popup.HorizontalOffset = (Window.Current.Bounds.Width - popup.ActualWidth) / 2;
                        popup.VerticalOffset = (Window.Current.Bounds.Height - popup.ActualHeight) / 2;
                    }
                }
            };
            popup.RegisterPropertyChangedCallback(Popup.IsOpenProperty, (d, e) =>
            {
                if (popup.IsOpen)
                {
                    Window.Current.SizeChanged += handler;
                }
                else
                {
                    Window.Current.SizeChanged -= handler;
                }
            });
            popup.Loaded += (s, e) => handler.Invoke(null, null);
            return popup;
        }

        public Popup Show(Popup popup)
        {
            popup.IsOpen = true;
            return popup;
        }

        public Popup Show(PopupSize size, UIElement content = null)
        {
            var popup = Create(size, content);
            return Show(popup);
        }

        public void SetContent(Popup popup, UIElement content) => popup.SetContent(content);

        public UIElement GetContent(Popup popup) => popup.GetContent();
    }
}
namespace Template10.Controls.PopupFacade
{
    public enum PopupPlacement { Fill, Center }

    public class PopupFacade
    {
        Popup popup;
        Window window;

        public PopupFacade()
        {
            popup = new Popup
            {
                Child = new ContentControl
                {
                    Padding = new Thickness(5),
                    Background = new SolidColorBrush(Colors.Gray),
                    BorderBrush = new SolidColorBrush(Colors.Red),
                    BorderThickness = new Windows.UI.Xaml.Thickness(2),

                }
            };
            popup.Opened += (s, e) => Opened?.Invoke(this, EventArgs.Empty);
            popup.Closed += (s, e) => Closed?.Invoke(this, EventArgs.Empty);
            window = Window.Current;
        }

        public event EventHandler Opened;
        public event EventHandler Closed;

        public void Show()
        {
            popup.IsOpen = true;
            Setup();
        }

        public void Hide()
        {
            popup.IsOpen = false;
            Teardown();
        }

        public Brush Background
        {
            set => (popup.Child as ContentControl).Background = value;
            get => (popup.Child as ContentControl).Background;
        }

        public Brush BorderBrush
        {
            set => (popup.Child as ContentControl).BorderBrush = value;
            get => (popup.Child as ContentControl).BorderBrush;
        }

        public Thickness BorderThickness
        {
            set => (popup.Child as ContentControl).BorderThickness = value;
            get => (popup.Child as ContentControl).BorderThickness;
        }

        public bool IsLightDismissEnabled
        {
            set => popup.IsLightDismissEnabled = value;
            get => popup.IsLightDismissEnabled;
        }

        public PopupPlacement PopupPlacement { get; set; } = PopupPlacement.Fill;

        public UIElement Content
        {
            set => (popup.Child as ContentControl).Content = value;
            get => (popup.Child as ContentControl).Content as UIElement;
        }

        private void Setup()
        {
            popup.Loaded += Popup_Loaded;
            //popup.LayoutUpdated += Popup_LayoutUpdated;
            //window.SizeChanged += Window_SizeChanged;
        }

        private void Teardown()
        {
            popup.Loaded -= Popup_Loaded;
            popup.LayoutUpdated -= Popup_LayoutUpdated;
            window.SizeChanged -= Window_SizeChanged;
        }

        private void Popup_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            Update();
        }

        private void Popup_LayoutUpdated(object sender, object e)
        {
            Update();
        }

        private void Update()
        {
            try
            {
                switch (PopupPlacement)
                {
                    case PopupPlacement.Fill:
                        {
                            popup.HorizontalOffset = 0;
                            popup.VerticalOffset = 0;

                            popup.Width = (Window.Current.Bounds.Width);
                            popup.Height = (Window.Current.Bounds.Height);

                            break;
                        }
                    case PopupPlacement.Center:
                        {
                            var actualHorizontalOffset = popup.HorizontalOffset;
                            var actualVerticalOffset = popup.VerticalOffset;

                            var newHorizontalOffset = (Window.Current.Bounds.Width - (popup.Child as ContentControl).ActualWidth) / 2;
                            var newVerticalOffset = (Window.Current.Bounds.Height - (popup.Child as ContentControl).ActualHeight) / 2;

                            if (actualHorizontalOffset != newHorizontalOffset || actualVerticalOffset != newVerticalOffset)
                            {
                                popup.HorizontalOffset = newHorizontalOffset;
                                popup.VerticalOffset = newVerticalOffset;
                            }

                            break;
                        }
                }
            }
            catch (Exception)
            {
                //                 throw;
            }
        }
    }
}