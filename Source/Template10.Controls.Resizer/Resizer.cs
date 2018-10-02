using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    [TemplatePart(Name = nameof(PART_THUMB), Type = typeof(Thumb))]
    [ContentProperty(Name = nameof(ElementControl))]
    public sealed class Resizer : ContentControl
    {
#pragma warning disable IDE1006 // Naming Styles
        private Thumb PART_THUMB;
#pragma warning restore IDE1006 // Naming Styles

        private Size _originalSize;
        public Resizer()
        {
            DefaultStyleKey = typeof(Resizer);
        }

        protected override void OnApplyTemplate()
        {
            PART_THUMB = GetTemplateChild<Thumb>(nameof(PART_THUMB)) ?? throw new NullReferenceException("PART_THUMB not found.");
            PART_THUMB.Loaded += (s, e) =>
            {
                _originalSize = ElementControl.RenderSize;
            };
            PART_THUMB.DragDelta += (s, e) =>
            {
                ElementControl.Width = Math.Max(0, ElementControl.ActualWidth + e.HorizontalChange); //HorizontalChange could become negative
                ElementControl.Height = Math.Max(0, ElementControl.ActualHeight + e.VerticalChange); //VerticalChange could become negative
            };
            PART_THUMB.DoubleTapped += (s, e) =>
            {
                ElementControl.Height = _originalSize.Height;
                ElementControl.Width = _originalSize.Width;
            };
        }

        public Control ElementControl
        {
            get => (Control)GetValue(ElementControlProperty) ?? throw new NullReferenceException("Content is required.");
            set => SetValue(ElementControlProperty, value);
        }
        public static readonly DependencyProperty ElementControlProperty =
            DependencyProperty.Register(nameof(ElementControl), typeof(Control),
                typeof(Resizer), new PropertyMetadata(default(Control)));

        public Visibility GrabberVisibility
        {
            get { return (Visibility)GetValue(GrabberVisibilityProperty); }
            set { SetValue(GrabberVisibilityProperty, value); }
        }

        public static readonly DependencyProperty GrabberVisibilityProperty =
            DependencyProperty.Register(nameof(GrabberVisibility), typeof(Visibility),
                typeof(Resizer), new PropertyMetadata(null));

        public double GrabberSize
        {
            get { return (double)GetValue(GrabberSizeProperty); }
            set { SetValue(GrabberSizeProperty, value); }
        }
        public static readonly DependencyProperty GrabberSizeProperty =
            DependencyProperty.Register(nameof(GrabberSize), typeof(double),
                typeof(Resizer), new PropertyMetadata(null));

        public Transform GrabberTransform
        {
            get { return (Transform)GetValue(GrabberTransformProperty); }
            set { SetValue(GrabberTransformProperty, value); }
        }
        public static readonly DependencyProperty GrabberTransformProperty =
            DependencyProperty.Register(nameof(GrabberTransform), typeof(Transform),
                typeof(Resizer), new PropertyMetadata(null));

        public Brush GrabberBrush
        {
            get { return (Brush)GetValue(GrabberBrushProperty); }
            set { SetValue(GrabberBrushProperty, value); }
        }
        public static readonly DependencyProperty GrabberBrushProperty =
            DependencyProperty.Register(nameof(GrabberBrush), typeof(Brush),
                typeof(Resizer), new PropertyMetadata(null));

        private T GetTemplateChild<T>(string name) where T : FrameworkElement
        {
            var child = GetTemplateChild(name) as T;
            if (child == null)
            {
                throw new NullReferenceException(name);
            }
            return child;
        }
    }
}
