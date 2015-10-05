using System.ComponentModel;
using Template10.Common;
using Windows.Foundation.Collections;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using System;
using Windows.UI.Xaml.Media;
using Template10.Utils;

namespace Template10.Controls
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-PageHeader
    [ContentProperty(Name = "PrimaryCommands")]
    public sealed partial class PageHeader : UserControl, INotifyPropertyChanged
    {
        public PageHeader()
        {
            this.InitializeComponent();
            PrimaryCommands = HeaderCommandBar.PrimaryCommands;
            SecondaryCommands = HeaderCommandBar.SecondaryCommands;
            this.Background = HeaderCommandBar.Background;
            HeaderCommandBar.SetBinding(CommandBar.BackgroundProperty, new Binding
            {
                Path = new PropertyPath(nameof(Background)),
                Source = this
            });

            Action updateEllipse = () =>
            {
                var controls = XamlUtil.AllChildren<Control>(HeaderCommandBar);
                var buttons = controls.OfType<Button>();
                var button = buttons.FirstOrDefault(x => x.Name.Equals("MoreButton"));
                if (button != null)
                {
                    var count = HeaderCommandBar.PrimaryCommands.OfType<Control>().Count(x => x.Visibility.Equals(Visibility.Visible));
                    count += HeaderCommandBar.SecondaryCommands.OfType<Control>().Count(x => x.Visibility.Equals(Visibility.Visible));
                    button.Visibility = (count > 0) ? Visibility.Visible : Visibility.Collapsed;
                }
            };
            PrimaryCommands.VectorChanged += (s, e) => updateEllipse();
            HeaderCommandBar.Loaded += (s, e) => updateEllipse();
        }

        public new object Content { get { return HeaderCommandBar.Content; } set { HeaderCommandBar.Content = value; } }

        #region VisualStateValues

        public double VisualStateNarrowMinWidth
        {
            get { return VisualStateNarrowTrigger.MinWindowWidth; }
            set { SetValue(VisualStateNarrowMinWidthProperty, VisualStateNarrowTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateNarrowMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNarrowMinWidth), typeof(double),
                typeof(PageHeader), new PropertyMetadata(null, (d, e) =>
                    { (d as PageHeader).VisualStateNarrowMinWidth = (double)e.NewValue; }));

        public double VisualStateNormalMinWidth
        {
            get { return VisualStateNormalTrigger.MinWindowWidth; }
            set { SetValue(VisualStateNormalMinWidthProperty, VisualStateNormalTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateNormalMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNormalMinWidth), typeof(double),
                typeof(PageHeader), new PropertyMetadata(null, (d, e) =>
                    { (d as PageHeader).VisualStateNormalMinWidth = (double)e.NewValue; }));

        #endregion

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty) ?? Resources["DefaultHeaderBackground"] as Brush; }
            set { SetValue(HeaderBackgroundProperty, value); }
        }
        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register(nameof(HeaderBackground), typeof(Brush),
                typeof(PageHeader), new PropertyMetadata(null));

        public Brush HeaderForeground
        {
            get { return (Brush)GetValue(HeaderForegroundProperty) ?? Resources["DefaultHeaderForeground"] as Brush; }
            set { SetValue(HeaderForegroundProperty, value); }
        }
        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.Register(nameof(HeaderForeground), typeof(Brush),
                typeof(PageHeader), new PropertyMetadata(null));

        public IObservableVector<ICommandBarElement> PrimaryCommands
        {
            get { return (IObservableVector<ICommandBarElement>)GetValue(PrimaryCommandsProperty); }
            private set { SetValue(PrimaryCommandsProperty, value); }
        }
        public static readonly DependencyProperty PrimaryCommandsProperty =
            DependencyProperty.Register(nameof(PrimaryCommands), typeof(IObservableVector<ICommandBarElement>),
                typeof(PageHeader), new PropertyMetadata(null));

        public IObservableVector<ICommandBarElement> SecondaryCommands
        {
            get { return (IObservableVector<ICommandBarElement>)GetValue(SecondaryCommandsProperty); }
            private set { SetValue(SecondaryCommandsProperty, value); }
        }
        public static readonly DependencyProperty SecondaryCommandsProperty =
            DependencyProperty.Register(nameof(SecondaryCommands), typeof(IObservableVector<ICommandBarElement>),
                typeof(PageHeader), new PropertyMetadata(null));

        public Frame Frame
        {
            get { return (Frame)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register(nameof(Frame), typeof(Frame),
                typeof(PageHeader), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string),
                typeof(PageHeader), new PropertyMetadata("Page Header"));

        public Visibility BackButtonVisibility
        {
            get { return (Visibility)GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty BackButtonVisibilityProperty =
            DependencyProperty.Register(nameof(BackButtonVisibility), typeof(Visibility),
                typeof(PageHeader), new PropertyMetadata(Visibility.Visible));

        public Boolean IsCommandBarOpen
        {
            get { return HeaderCommandBar.IsOpen; }
            set { SetValue(IsCommandBarOpenProperty, HeaderCommandBar.IsOpen = value); }
        }
        public static readonly DependencyProperty IsCommandBarOpenProperty =
            DependencyProperty.Register(nameof(IsCommandBarOpen), typeof(Boolean),
                typeof(PageHeader), new PropertyMetadata(false, (d, e) =>
                { (d as PageHeader).IsCommandBarOpen = (Boolean)e.NewValue; }));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
