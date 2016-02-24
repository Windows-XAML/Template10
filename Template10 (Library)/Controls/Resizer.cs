using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    [TemplatePart(Name = nameof(PART_ROOT), Type = typeof(Grid))]
    [TemplatePart(Name = nameof(PART_THUMB), Type = typeof(Thumb))]
    [TemplatePart(Name = nameof(PART_GRABBER), Type = typeof(Grid))]
    [TemplatePart(Name = nameof(PART_CONTENT), Type = typeof(ContentControl))]
    [ContentProperty(Name = nameof(ElementControl))]
    public sealed class Resizer : ContentControl
    {
        private Thumb PART_THUMB;
        private ContentControl PART_CONTENT;
        private Grid PART_GRABBER;
        private Grid PART_ROOT;

        private Size _originalSize;
        public Resizer()
        {
            DefaultStyleKey = typeof(Resizer);
        }

        protected override void OnApplyTemplate()
        {
            PART_ROOT = GetTemplateChild<Grid>(nameof(PART_ROOT));
            PART_GRABBER = GetTemplateChild<Grid>(nameof(PART_GRABBER));
            PART_CONTENT = GetTemplateChild<ContentControl>(nameof(PART_CONTENT));
            PART_THUMB = GetTemplateChild<Thumb>(nameof(PART_THUMB));

            InitEvents();
        }

        private T GetTemplateChild<T>(string name) where T : FrameworkElement
        {
            var child = GetTemplateChild(name) as T;
            if (child == null)
                throw new NullReferenceException(name);
            return child;
        }

        private void InitEvents()
        {
            PART_THUMB.Loaded += thumb_Loaded;
            PART_THUMB.DragDelta += thumb_DragDelta;
            PART_THUMB.DoubleTapped += thumb_DoubleTapped;
        }

        private void thumb_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ElementControl.Height = _originalSize.Height;
            ElementControl.Width = _originalSize.Width;
        }

        private void thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ElementControl.Width = Math.Max(0, ElementControl.ActualWidth + e.HorizontalChange); //HorizontalChange could become negative
            ElementControl.Height = Math.Max(0, ElementControl.ActualHeight + e.VerticalChange); //VerticalChange could become negative
        }

        private void thumb_Loaded(object sender, RoutedEventArgs e)
        {
            _originalSize = ElementControl.RenderSize;
        }


        public Control ElementControl
        {
            get { return (Control)GetValue(ElementControlProperty); }
            set { SetValue(ElementControlProperty, value); }
        }

        public static readonly DependencyProperty ElementControlProperty =
            DependencyProperty.Register(nameof(ElementControl), typeof(Control), typeof(Resizer), new PropertyMetadata(default(Control)));

        public Visibility GrabberVisibility
        {
            get { return (Visibility)GetValue(GrabberVisibilityProperty); }
            set { SetValue(GrabberVisibilityProperty, value); }
        }

        public static readonly DependencyProperty GrabberVisibilityProperty =
            DependencyProperty.Register(nameof(GrabberVisibility), typeof(Visibility), typeof(Resizer), new PropertyMetadata(default(Visibility)));



        public Size GrabberSize
        {
            get { return PART_GRABBER.RenderSize; }
            set
            {
                PART_GRABBER.Width = value.Width;
                PART_GRABBER.Height = value.Height;

                // move it
                var transform = PART_GRABBER.RenderTransform as CompositeTransform;
                if (transform != null)
                {
                    transform.TranslateX = value.Width * .3;
                    transform.TranslateY = value.Height * .3;
                }
            }
        }

        public static readonly DependencyProperty GrabberSizeProperty =
            DependencyProperty.Register(nameof(GrabberSize), typeof(Size), typeof(Resizer), new PropertyMetadata(default(Size)));


        public Brush GrabberBrush
        {
            get { return (Brush)GetValue(GrabberBrushProperty); }
            set { SetValue(GrabberBrushProperty, value); }
        }

        public static readonly DependencyProperty GrabberBrushProperty =
            DependencyProperty.Register(nameof(GrabberBrush), typeof(Brush), typeof(Resizer), new PropertyMetadata(default(Brush)));

    }
}
