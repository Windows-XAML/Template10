using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Template10.Services.Overlay
{

    public static class Extensions
    {
        public static UIElement GetContent(this Popup popup) => (popup.Child as ContentControl).Content as UIElement;
        public static void SetContent(this Popup popup, UIElement newContent)
        {
            var contentControl = popup.Child as ContentControl;
            contentControl.Height = Window.Current.Bounds.Height;
            contentControl.Width = Window.Current.Bounds.Width;
            contentControl.Content = newContent;
        }

        public static void Show(this Popup popup, UIElement newContent)
        {
            SetContent(popup, newContent);
            Show(popup);
        }
        public static void Show(this Popup popup) => popup.IsOpen = true;
        public static void Hide(this Popup popup) => popup.IsOpen = false;
        public static void Hide(this Popup popup, UIElement newContent)
        {
            SetContent(popup, newContent);
            Hide(popup);
        }
    }

}