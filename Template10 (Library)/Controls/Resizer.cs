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
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Template10.Controls
{
    [TemplatePart(Name = PART_ROOT_NAME, Type = typeof(Grid))]
    [TemplatePart(Name = PART_THUMB_NAME, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_GRABBER_NAME, Type = typeof(Grid))]
    [TemplatePart(Name = PART_CONTENT_NAME, Type = typeof(ContentPresenter))]
    public sealed class Resizer : ContentControl
    {
        private const string PART_THUMB_NAME = "PART_THUMB";
        private const string PART_CONTENT_NAME = "PART_CONTENT";
        private const string PART_GRABBER_NAME = "PART_GRABBER";
        private const string PART_ROOT_NAME = "PART_ROOT";
        private Grid _rootGrid;
        private Grid _grabberGrid;
        private ContentPresenter _contentPresenter;
        private Thumb _thumb;
        private Size originalSize;
        public Resizer()
        {
            this.DefaultStyleKey = typeof(Resizer);
        }

        protected override void OnApplyTemplate()
        {
            _rootGrid = GetTemplateChild(PART_ROOT_NAME) as Grid;
            _grabberGrid = GetTemplateChild(PART_GRABBER_NAME) as Grid;
            _contentPresenter = GetTemplateChild(PART_CONTENT_NAME) as ContentPresenter;
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
            _contentPresenter.Height = originalSize.Height;
            _contentPresenter.Width = originalSize.Width;
        }

        private void thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _contentPresenter.Width = Math.Max(0, _contentPresenter.ActualWidth + e.HorizontalChange); //HorizontalChange could become negative
            _contentPresenter.Height = Math.Max(0, _contentPresenter.ActualHeight + e.VerticalChange); //VerticalChange could become negative
        }

        private void thumb_Loaded(object sender, RoutedEventArgs e)
        {
            originalSize = _contentPresenter.RenderSize;
        }


        public Control ElementControl
        {
            get { return (Control)GetValue(ElementControlProperty); }
            set { SetValue(ElementControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElementControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementControlProperty =
            DependencyProperty.Register("ElementControl", typeof(Control), typeof(Resizer), new PropertyMetadata(default(Control)));

        public Visibility GrabberVisibility
        {
            get { return (Visibility)GetValue(GrabberVisibilityProperty); }
            set { SetValue(GrabberVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GrabberVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GrabberVisibilityProperty =
            DependencyProperty.Register("GrabberVisibility", typeof(Visibility), typeof(Resizer), new PropertyMetadata(default(Visibility)));



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

        // Using a DependencyProperty as the backing store for GrabberSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GrabberSizeProperty =
            DependencyProperty.Register("GrabberSize", typeof(Size), typeof(Resizer), new PropertyMetadata(default(Size)));


        public Brush GrabberBrush
        {
            get { return (Brush)GetValue(GrabberBrushProperty); }
            set { SetValue(GrabberBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GrabberBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GrabberBrushProperty =
            DependencyProperty.Register("GrabberBrush", typeof(Brush), typeof(Resizer), new PropertyMetadata(default(Brush)));



    }
}
