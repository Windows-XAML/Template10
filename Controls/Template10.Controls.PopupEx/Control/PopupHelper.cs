using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Template10.Services.PopupService
{
    public class PopupHelper
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
