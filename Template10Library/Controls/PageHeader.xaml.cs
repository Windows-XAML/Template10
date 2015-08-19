using System.ComponentModel;
using Template10.Common;
using Windows.Foundation.Collections;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using System;

namespace Template10.Controls
{
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

        public IObservableVector<ICommandBarElement> PrimaryCommands
        {
            get { return (IObservableVector<ICommandBarElement>)GetValue(PrimaryCommandsProperty); }
            private set { SetValue(PrimaryCommandsProperty, value); }
        }
        public static readonly DependencyProperty PrimaryCommandsProperty =
            DependencyProperty.Register("PrimaryCommands", typeof(IObservableVector<ICommandBarElement>),
                typeof(PageHeader), new PropertyMetadata(null));

        public IObservableVector<ICommandBarElement> SecondaryCommands
        {
            get { return (IObservableVector<ICommandBarElement>)GetValue(SecondaryCommandsProperty); }
            private set { SetValue(SecondaryCommandsProperty, value); }
        }
        public static readonly DependencyProperty SecondaryCommandsProperty =
            DependencyProperty.Register("SecondaryCommands", typeof(IObservableVector<ICommandBarElement>),
                typeof(PageHeader), new PropertyMetadata(null));

        public Frame Frame
        {
            get { return (Frame)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frame))); }
        }
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register("Frame", typeof(Frame),
                typeof(PageHeader), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text))); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string),
                typeof(PageHeader), new PropertyMetadata("Page Header"));

        public Visibility BackButtonVisibility
        {
            get { return (Visibility)GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackButtonVisibility))); }
        }
        public static readonly DependencyProperty BackButtonVisibilityProperty =
            DependencyProperty.Register("BackButtonVisibility", typeof(Visibility),
                typeof(PageHeader), new PropertyMetadata(Visibility.Visible));

        public Visibility ForwardButtonVisibility
        {
            get { return (Visibility)GetValue(ForwardButtonVisibilityProperty); }
            set { SetValue(ForwardButtonVisibilityProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ForwardButtonVisibility))); }
        }

        public static readonly DependencyProperty ForwardButtonVisibilityProperty =
            DependencyProperty.Register("ForwardButtonVisibility", typeof(Visibility),
                typeof(PageHeader), new PropertyMetadata(Visibility.Visible));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
