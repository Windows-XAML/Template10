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

            // TODO
            Action updateEllipse = () =>
            {
                var controls = XamlHelper.AllChildren<Control>(HeaderCommandBar);
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

        #region VisualStateValues

        public double VisualStateNarrowMinWidth
        {
            get { return VisualStateNarrowTrigger.MinWindowWidth; }
            set { SetValue(VisualStateNarrowMinWidthProperty, VisualStateNarrowTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateNarrowMinWidthProperty =
            DependencyProperty.Register("VisualStateNarrowMinWidth", typeof(double), typeof(PageHeader), new PropertyMetadata(null, (d, e) => { (d as PageHeader).VisualStateNarrowMinWidth = (double)e.NewValue; }));
        public double VisualStateMediumMinWidth
        {
            get { return VisualStateMediumTrigger.MinWindowWidth; }
            set { SetValue(VisualStateMediumMinWidthProperty, VisualStateMediumTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateMediumMinWidthProperty =
            DependencyProperty.Register("VisualStateMediumMinWidth", typeof(double), typeof(PageHeader), new PropertyMetadata(null, (d, e) => { (d as PageHeader).VisualStateMediumMinWidth = (double)e.NewValue; }));
        public double VisualStateWideMinWidth
        {
            get { return VisualStateWideTrigger.MinWindowWidth; }
            set { SetValue(VisualStateWideMinWidthProperty, VisualStateWideTrigger.MinWindowWidth = value); }
        }
        public static readonly DependencyProperty VisualStateWideMinWidthProperty =
            DependencyProperty.Register("VisualStateWideMinWidth", typeof(double), typeof(PageHeader), new PropertyMetadata(null, (d, e) => { (d as PageHeader).VisualStateWideMinWidth = (double)e.NewValue; }));

        #endregion

        #region Styles

        public SolidColorBrush HeaderBackground
        {
            get { return HeaderBackgroundBrush; }
            set
            {
                SetValue(HeaderBackgroundProperty, HeaderBackgroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeaderBackground)));
            }
        }
        public static readonly DependencyProperty HeaderBackgroundProperty =
             DependencyProperty.Register(nameof(HeaderBackground), typeof(SolidColorBrush),
                 typeof(PageHeader), new PropertyMetadata(null, (d, e) => { (d as PageHeader).HeaderBackground = (SolidColorBrush)e.NewValue; }));

        public SolidColorBrush HeaderForeground
        {
            get { return HeaderForegroundBrush; }
            set
            {
                SetValue(HeaderForegroundProperty, HeaderForegroundBrush = value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeaderForeground)));
            }
        }
        public static readonly DependencyProperty HeaderForegroundProperty =
              DependencyProperty.Register(nameof(HeaderForeground), typeof(SolidColorBrush),
                  typeof(PageHeader), new PropertyMetadata(null, (d, e) => { (d as PageHeader).HeaderForeground = (SolidColorBrush)e.NewValue; }));

        #endregion

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
            set { SetValue(FrameProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frame))); }
        }
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register(nameof(Frame), typeof(Frame),
                typeof(PageHeader), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text))); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string),
                typeof(PageHeader), new PropertyMetadata("Page Header"));

        public Visibility BackButtonVisibility
        {
            get { return (Visibility)GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackButtonVisibility))); }
        }
        public static readonly DependencyProperty BackButtonVisibilityProperty =
            DependencyProperty.Register(nameof(BackButtonVisibility), typeof(Visibility),
                typeof(PageHeader), new PropertyMetadata(Visibility.Visible));

        public Visibility ForwardButtonVisibility
        {
            get { return (Visibility)GetValue(ForwardButtonVisibilityProperty); }
            set { SetValue(ForwardButtonVisibilityProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ForwardButtonVisibility))); }
        }
        public static readonly DependencyProperty ForwardButtonVisibilityProperty =
            DependencyProperty.Register(nameof(ForwardButtonVisibility), typeof(Visibility),
                typeof(PageHeader), new PropertyMetadata(Visibility.Visible));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
