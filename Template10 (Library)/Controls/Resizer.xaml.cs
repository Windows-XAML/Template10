using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    public sealed partial class Resizer : UserControl
    {
        public Resizer()
        {
            this.InitializeComponent();
        }

        public Control Element
        {
            get { return MyContentControl.Content as Control; }
            set { MyContentControl.Content = value; }
        }
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register(nameof(Element), typeof(Control),
                typeof(Resizer), new PropertyMetadata(null, (d, e) =>
                { (d as Resizer).Element = e.NewValue as Control; }));

        public Visibility GrabberVisibility
        {
            get { return MyGrabber.Visibility; }
            set { MyGrabber.Visibility = value; }
        }
        public static readonly DependencyProperty GrabberVisibilityProperty =
            DependencyProperty.Register(nameof(GrabberVisibility), typeof(Visibility),
                typeof(Resizer), new PropertyMetadata(Visibility.Visible, (d, e) =>
                { (d as Resizer).GrabberVisibility = (Visibility)e.NewValue; }));

        public Size GrabberSize
        {
            get { return MyGrabber.RenderSize; }
            set
            {
                MyGrabber.Width = value.Width;
                MyGrabber.Height = value.Height;
                var transform = MyGrabber.RenderTransform as CompositeTransform;
                transform.TranslateX = value.Width * .3;
                transform.TranslateY = value.Height * .3;
            }
        }
        public static readonly DependencyProperty GrabberSizeProperty =
            DependencyProperty.Register("GrabberSize", typeof(Size),
                typeof(Resizer), new PropertyMetadata(new Size(30, 30), (d, e) =>
                { (d as Resizer).GrabberSize = (Size)e.NewValue; }));

        Size originalSize;
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
