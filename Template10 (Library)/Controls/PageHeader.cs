using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Template10.Utils;
using System.Collections.Specialized;
using System.Collections;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Template10.Controls
{
    [ContentProperty(Name = nameof(PrimaryCommands))]
    public sealed class PageHeader : CommandBar
    {
        public PageHeader()
        {
            this.DefaultStyleKey = typeof(PageHeader);
        }

        public Visibility BackButtonVisibility
        {
            get { return (Visibility)GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty BackButtonVisibilityProperty =
            DependencyProperty.Register(nameof(BackButtonVisibility), typeof(Visibility), typeof(PageHeader), new PropertyMetadata(Visibility.Collapsed));

        private Symbol BackButtonContent
        {
            get { return (Symbol)GetValue(BackButtonContentProperty); }
            set { SetValue(BackButtonContentProperty, value); }
        }
        private static readonly DependencyProperty BackButtonContentProperty =
            DependencyProperty.Register(nameof(BackButtonContent), typeof(Symbol), typeof(PageHeader),
                new PropertyMetadata(Symbol.Back));

        public Frame Frame
        {
            get { return (Frame)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }

        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register(nameof(Frame), typeof(Frame), typeof(PageHeader), new PropertyMetadata(default(Frame), OnFramePropertyChanged));

        private static void OnFramePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frame = (Frame)e.NewValue;

            if (frame.FlowDirection == FlowDirection.LeftToRight)
                d.SetValue(BackButtonContentProperty, Symbol.Back);
            else
                d.SetValue(BackButtonContentProperty, Symbol.Forward);
        }

        public double VisualStateNarrowMinWidth
        {
            get { return (double)GetValue(VisualStateNarrowMinWidthProperty); }
            set { SetValue(VisualStateNarrowMinWidthProperty, value); }
        }
        public static readonly DependencyProperty VisualStateNarrowMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNarrowMinWidth), typeof(double), typeof(PageHeader), new PropertyMetadata(-1));

        public double VisualStateNormalMinWidth
        {
            get { return (double)GetValue(VisualStateNormalMinWidthProperty); }
            set { SetValue(VisualStateNormalMinWidthProperty, value); }
        }
        public static readonly DependencyProperty VisualStateNormalMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNormalMinWidth), typeof(double), typeof(PageHeader), new PropertyMetadata(0));

        public new object Content
        {
            get { return base.Content; }
            set { base.Content = value; }
        }

        public string Text
        {
            get { return Content as string; }
            set { Content = value; }
        }
    }
}
