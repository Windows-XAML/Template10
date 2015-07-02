using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    public sealed class ForwardButton : Button
    {
        public event EventHandler VisibilityChanged;

        public ForwardButton()
        {
            this.RenderTransform = new ScaleTransform { ScaleX = -1 };
            this.Style = this.Resources["NavigationBackButtonNormalStyle"] as Style;
            this.DefaultStyleKey = typeof(ForwardButton);
            Loaded += (s, e) =>
            {
                DependencyObject item = this;
                while (!((item = VisualTreeHelper.GetParent(item)) is Page)) { }
                Page page = item as Page;
                this.Frame = page.Frame;
                this.Visibility = CalculateOnCanvasBackVisibility();
            };
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                this.Visibility = Visibility.Visible;
                return;
            }
            this.Click += (s, e) => Frame.GoForward();
            Window.Current.SizeChanged += (s, arg) => this.Visibility = CalculateOnCanvasBackVisibility();
            RegisterPropertyChangedCallback(VisibilityProperty, (s, e) => VisibilityChanged?.Invoke(this, EventArgs.Empty));
        }

        public Frame Frame { get; private set; }

        private Visibility CalculateOnCanvasBackVisibility()
        {
            // by design it is not visible when not applicable
            var cangoforward = Frame.CanGoForward;
            if (!cangoforward)
                return Visibility.Collapsed;

            // at this point, we show the on-canvas button
            return Visibility.Visible;
        }
    }
}
