using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Template10.Controls
{
    [TemplatePart(Name = PART_ROOT_NAME, Type = typeof(Grid))]
    [TemplatePart(Name = PART_THUMB_NAME, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_GRABBER_NAME, Type = typeof(Grid))]
    [TemplatePart(Name = PART_CONTENT_NAME, Type = typeof(ContentControl))]
    [ContentProperty(Name = nameof(ElementControl))]
    public sealed class Resizer : ContentControl
    {
        private const string PART_THUMB_NAME = "PART_THUMB";
        private const string PART_CONTENT_NAME = "PART_CONTENT";
        private const string PART_GRABBER_NAME = "PART_GRABBER";
        private const string PART_ROOT_NAME = "PART_ROOT";
        private Grid _rootGrid;
        private Grid _grabberGrid;
        private ContentControl _contentPresenter;
        private Thumb _thumb;
        private Size _originalSize;
        public Resizer()
        {
            this.DefaultStyleKey = typeof(Resizer);
        }

        protected override void OnApplyTemplate()
        {
            _rootGrid = GetTemplateChild(PART_ROOT_NAME) as Grid;
            _grabberGrid = GetTemplateChild(PART_GRABBER_NAME) as Grid;
            _contentPresenter = GetTemplateChild(PART_CONTENT_NAME) as ContentControl;
            _thumb = GetTemplateChild(PART_THUMB_NAME) as Thumb;

            InitEvents();
        }

        private void InitEvents()
        {
            _thumb.Loaded += thumb_Loaded;
            _thumb.DragDelta += thumb_DragDelta;
            _thumb.DoubleTapped += thumb_DoubleTapped;
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
            get { return _grabberGrid.RenderSize; }
            set
            {
                _grabberGrid.Width = value.Width;
                _grabberGrid.Height = value.Height;

                // move it
                var transform = _grabberGrid.RenderTransform as CompositeTransform;
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
