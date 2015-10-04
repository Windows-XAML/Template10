using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    [ContentProperty(Name = nameof(Element))]
    public sealed partial class Resizer : UserControl
    {
        public Resizer()
        {
            this.InitializeComponent();
        }

        public Control Element
        {
            get { return (Control)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(Control), 
                typeof(Resizer), new PropertyMetadata(null));

        public Visibility GrabberVisibility
        {
            get { return (Visibility)GetValue(GrabberVisibilityProperty); }
            set { SetValue(GrabberVisibilityProperty, value); }
        }
        public static readonly DependencyProperty GrabberVisibilityProperty =
            DependencyProperty.Register("GrabberVisibility", typeof(Visibility), 
                typeof(Resizer), new PropertyMetadata(Visibility.Visible));

        public Brush GrabberBrush
        {
            get { return (Brush)GetValue(GrabberBrushProperty) ?? Resources["SystemAccentBrush"] as SolidColorBrush; }
            set { SetValue(GrabberBrushProperty, value); }
        }
        public static readonly DependencyProperty GrabberBrushProperty =    
            DependencyProperty.Register("GrabberBrush", typeof(Brush), typeof(Resizer), 
                new PropertyMetadata(null));

        Windows.Foundation.Size originalSize;
        private void GrabLoaded(object sender, RoutedEventArgs e)
        {
            originalSize = Element.RenderSize;
        }

        private void GrabDelta(object sender, Windows.UI.Xaml.Controls.Primitives.DragDeltaEventArgs e)
        {
            Element.Width = Element.ActualWidth + e.HorizontalChange;
            Element.Height = Element.ActualHeight + e.VerticalChange;
        }

        private void GrabDoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            Element.Height = originalSize.Height;
            Element.Width = originalSize.Width;
        }
    }
}
