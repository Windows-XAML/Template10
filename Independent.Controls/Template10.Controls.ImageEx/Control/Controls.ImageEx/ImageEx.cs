using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
	public sealed class ImageEx : Control
    {
        public ImageEx()
        {
            this.DefaultStyleKey = typeof(ImageEx);
        }

        #region Facade members

        /// <summary>
        /// When ImageOpened fires, that serves as the notification that any asynchronous operations have completed and all the properties of the object used as the image source are ready for use. For example, to determine the size of the image, handle ImageOpened, and check the value of the PixelWidth and PixelHeight properties on the object referenced as Image.Source. The event data for the ImageOpened event isn't typically useful.
        /// </summary>
        public event RoutedEventHandler ImageOpened;

        /// <summary>
        /// Conditions in which this event can occur include: File not found, Invalid(unrecognized or unsupported) file format, Unknown file format decoding error after upload, Qualified resource reload by the system You might be able to use the ErrorMessage in event data to determine the nature of the failure. ImageFailed and ImageOpened are mutually exclusive.One event or the other will always file whenever an Image has a Source value set or reset.
        /// </summary>
        public event RoutedEventHandler ImageFailed;

        /// <summary>
        /// Setting the Source property is inherently an asynchronous action. Because it's a property, there isn't an awaitable syntax, but for most scenarios you don't need to interact with the asynchronous aspects of image source file loading. The framework will wait for the image source to be returned, and will rerun layout when the image source file becomes available.
        /// </summary>
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(ImageSource),
                typeof(ImageEx), new PropertyMetadata(null));

        /// <summary>
        /// The value of the Stretch property is only relevant if the Image instance is not already using explicitly set values for the Height and/or Width property, and if the Image instance is inside a container that can stretch the image to fill some available space in layout. If you set the value of the Stretch property to None, the image always retains its natural size, even if there's a layout container that might stretch it otherwise. For more info on image sizing, see Remarks in Image. 
        /// </summary>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch),
                typeof(ImageEx), new PropertyMetadata(Stretch.UniformToFill));

        #endregion  

        Image part_imageForeground;
        Image part_imageBackground;

        protected override void OnApplyTemplate()
        {
            part_imageForeground = (Image)GetTemplateChild(nameof(part_imageForeground));
            part_imageForeground.ImageOpened += (s, e) => ImageOpened?.Invoke(this, new RoutedEventArgs());
            part_imageBackground = (Image)GetTemplateChild(nameof(part_imageBackground));
            part_imageBackground.ImageOpened += (s, e) => part_imageForeground.Source = part_imageBackground.Source;
            part_imageBackground.ImageFailed += (s, e) => ImageFailed?.Invoke(this, new RoutedEventArgs());
        }
    }
}
