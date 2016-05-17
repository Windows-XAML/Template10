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
        private static long? callBackIsOpenId;
        private static long? callBackDisplayModeId;
        public PageHeader()
        {
            DefaultStyleKey = typeof(PageHeader);

            // behaviors
            var behavior = new Behaviors.EllipsisBehavior();
            RegisterPropertyChangedCallback(EllipsisVisibilityProperty, (s, e) => behavior.Visibility = EllipsisVisibility);
            var collection = new Microsoft.Xaml.Interactivity.BehaviorCollection();
            collection.Add(behavior);
            SetValue(Microsoft.Xaml.Interactivity.Interaction.BehaviorsProperty, collection);

            TabIndex = 5000;
            Loaded += PageHeader_Loaded;
        }

        private void PageHeader_Loaded(object sender, RoutedEventArgs e)
        {
            var page = this.GetFirstAncestorOfType<Page>();
            var frame = page?.Frame;
            var hamburgerMenu = frame?.GetFirstAncestorOfType<HamburgerMenu>();

            hamburgerMenu?.UnregisterPropertyChangedCallback(HamburgerMenu.IsOpenProperty, callBackIsOpenId ?? 0);
            callBackIsOpenId = hamburgerMenu?.RegisterPropertyChangedCallback(HamburgerMenu.IsOpenProperty, (obj, dp) => OnHamburgerMenuIsOpenPropertyChanged(obj as HamburgerMenu));

            hamburgerMenu?.UnregisterPropertyChangedCallback(HamburgerMenu.DisplayModeProperty, callBackDisplayModeId ?? 0);
            callBackDisplayModeId = hamburgerMenu?.RegisterPropertyChangedCallback(HamburgerMenu.DisplayModeProperty, (obj, dp) => OnHamburgerMenuDisplayModePropertyChanged(obj as HamburgerMenu));

            //******************************************************************************************************
            // Temp fix till HM VisualState Manager is checked out.
            // HamburgerMenu.xaml.cs/VisualStateGroup_CurrentStateChanged is expected to fire on sartup but doeesn't
            // Test with this to verify:
            // Set initial value for DisplayMode DP PropertyMetadata to SplitViewDisplayMode.Inline (for this test
            // comment out any code-behind or XAML setting of DisplayMode elsewhere, usually in Shell) and see if 
            // VisualStateGroup_CurrentStateChanged triggers for Windows Phone.
            // VisualState Manager should setDisplayMode to Overlay but stays Inline!
            //******************************************************************************************************

            if (hamburgerMenu.DisplayMode == SplitViewDisplayMode.Inline)
            {
                Margin = (page.ActualWidth <= VisualStateNormalMinWidth) ? new Thickness(0) : new Thickness(48, 0, 0, 0);

            }else if(hamburgerMenu.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                Margin = (page.ActualWidth <= VisualStateNormalMinWidth) ? new Thickness(0) : new Thickness(48, 0, 0, 0);

            }
            else
            {
                Margin = (page.ActualWidth <= VisualStateNormalMinWidth) ? new Thickness(-48, 0, 0, 0) : new Thickness(0);

            }
        }
        private void OnHamburgerMenuDisplayModePropertyChanged(HamburgerMenu hamburgerMenu)
        {
            Margin = (hamburgerMenu.DisplayMode == SplitViewDisplayMode.Inline)
           ? new Thickness(48, 0, 0, 0) : new Thickness(0);
        }

        private void OnHamburgerMenuIsOpenPropertyChanged(HamburgerMenu hamburgerMenu)
        {

            if (hamburgerMenu.DisplayMode == SplitViewDisplayMode.Inline)
            {
                if (hamburgerMenu.ActualWidth == 500)
                {
                    Margin = (hamburgerMenu.IsOpen) ? new Thickness(-48, 0, 0, 0) : new Thickness(0);
                }
                else
                {
                    Margin = (hamburgerMenu.IsOpen) ? new Thickness(0) : new Thickness(48, 0, 0, 0);
                }
            }
            else if (hamburgerMenu.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                Margin = (hamburgerMenu.ActualWidth <= 500) ? new Thickness(0, 0, 0, 0) : new Thickness(48, 0, 0, 0);

            }
            else
            {
                Margin = (hamburgerMenu.ActualWidth == 500) ? new Thickness(-48, 0, 0, 0) : new Thickness(0);

            }
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
                typeof(PageHeader), new PropertyMetadata((double)-1));

        public double VisualStateNormalMinWidth
        {
            get { return (double)GetValue(VisualStateNormalMinWidthProperty); }
            set { SetValue(VisualStateNormalMinWidthProperty, value); }
        }
        public static readonly DependencyProperty VisualStateNormalMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNormalMinWidth), typeof(double),
                typeof(PageHeader), new PropertyMetadata(0d));

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


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            moreButton = GetTemplateChild("MoreButton") as Button;
        }

        private Button moreButton;
        internal Button GetMoreButton() => moreButton;
    }
}
