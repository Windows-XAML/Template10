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
            DefaultStyleKey = typeof(PageHeader);

            // behaviors
            var b = new Behaviors.EllipsisBehavior();
            RegisterPropertyChangedCallback(EllipsisVisibilityProperty, (s, e) => b.Visibility = EllipsisVisibility);
            var c = new Microsoft.Xaml.Interactivity.BehaviorCollection();
            c.Add(b);
            SetValue(Microsoft.Xaml.Interactivity.Interaction.BehaviorsProperty, c);

            TabIndex = 5000;
        }

        public Behaviors.EllipsisBehavior.Visibilities EllipsisVisibility
        {
            get { return (Behaviors.EllipsisBehavior.Visibilities)GetValue(EllipsisVisibilityProperty); }
            set { SetValue(EllipsisVisibilityProperty, value); }
        }
        public static readonly DependencyProperty EllipsisVisibilityProperty =
            DependencyProperty.Register(nameof(EllipsisVisibility), typeof(Behaviors.EllipsisBehavior.Visibilities),
                typeof(PageHeader), new PropertyMetadata(Behaviors.EllipsisBehavior.Visibilities.Auto));

        public Visibility PrimaryCommandsVisibility
        {
            get { return (Visibility)GetValue(PrimaryCommandsVisibilityProperty); }
            set { SetValue(PrimaryCommandsVisibilityProperty, value); }
        }
        public static readonly DependencyProperty PrimaryCommandsVisibilityProperty =
            DependencyProperty.Register(nameof(PrimaryCommandsVisibility), typeof(Visibility),
                typeof(PageHeader), new PropertyMetadata(Visibility.Visible));

        public Visibility BackButtonVisibility
        {
            get { return (Visibility)GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty BackButtonVisibilityProperty =
            DependencyProperty.Register(nameof(BackButtonVisibility), typeof(Visibility),
                typeof(PageHeader), new PropertyMetadata(Visibility.Collapsed));

        public Symbol BackButtonContent
        {
            get { return (Symbol)GetValue(BackButtonContentProperty); }
            set { SetValue(BackButtonContentProperty, value); }
        }
        public static readonly DependencyProperty BackButtonContentProperty =
            DependencyProperty.Register(nameof(BackButtonContent), typeof(Symbol), typeof(PageHeader),
                new PropertyMetadata(Symbol.Back));

        public Frame Frame { get { return (Frame)GetValue(FrameProperty); } set { SetValue(FrameProperty, value); } }
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register(nameof(Frame), typeof(Frame), typeof(PageHeader),
                new PropertyMetadata(default(Frame), OnFramePropertyChanged));
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
            DependencyProperty.Register(nameof(VisualStateNarrowMinWidth), typeof(double),
                typeof(PageHeader), new PropertyMetadata(-1));

        public double VisualStateNormalMinWidth
        {
            get { return (double)GetValue(VisualStateNormalMinWidthProperty); }
            set { SetValue(VisualStateNormalMinWidthProperty, value); }
        }
        public static readonly DependencyProperty VisualStateNormalMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNormalMinWidth), typeof(double),
                typeof(PageHeader), new PropertyMetadata(0));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PageHeader), new PropertyMetadata(string.Empty, (d, e) =>
            {
                (d as PageHeader).Content = e.NewValue;
            }));
    }
}
