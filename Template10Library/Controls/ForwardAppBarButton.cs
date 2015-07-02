using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    public sealed class ForwardAppBarButton : AppBarButton
    {
        public event EventHandler VisibilityChanged;

        public ForwardAppBarButton()
        {
            this.Label = "Forward";
            this.Content = new FontIcon
            {
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Glyph = Common.Mdl2.ArrowRight
            };

            this.DefaultStyleKey = typeof(AppBarButton);
            Loaded += (s, e) =>
            {
                if (this.Frame == null) { throw new NullReferenceException("Please set Frame property"); }
                this.Visibility = CalculateOnCanvasBackVisibility();
            };
            this.RenderTransform = new ScaleTransform { ScaleX = -1 };
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                this.Visibility = Visibility.Visible;
                return;
            }
            this.Click += (s, e) => Frame.GoForward();
            Window.Current.SizeChanged += (s, arg) => this.Visibility = CalculateOnCanvasBackVisibility();
            RegisterPropertyChangedCallback(VisibilityProperty, (s, e) => VisibilityChanged?.Invoke(this, EventArgs.Empty));
        }

        public Frame Frame { get; set; }

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
