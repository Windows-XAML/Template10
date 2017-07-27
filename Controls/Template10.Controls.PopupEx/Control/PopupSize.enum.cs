using System;
using Template10.Controls.Popup;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Template10.Services.PopupService
{
    public enum PopupSize { FullScreen, ContentBased }
}
namespace Template10.Controls.PopupFacade
{
    public class Template10Popup
    {
        Windows.UI.Xaml.Controls.Primitives.Popup popup;
        Window window;

        public Template10Popup()
        {
            popup = new Windows.UI.Xaml.Controls.Primitives.Popup
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