using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Template10.Services.Overlay
{
    public class OverlayService
    {
        public Popup Create(Sizes size, UIElement content = null)
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
                    container.Height = e.Size.Height;
                    container.Width = e.Size.Width;
                }
            };
            if (size == Sizes.FullScreen)
            {
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
            }
            return popup;
        }

        public Popup Show(Sizes size, UIElement content = null)
        {
            var popup = Create(size, content);
            popup.IsOpen = true;
            return popup;
        }

        public void SetContent(Popup popup, UIElement content) => popup.SetContent(content);
        public UIElement GetContent(Popup popup) => popup.GetContent();
    }

}
