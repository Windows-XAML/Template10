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

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Template10.Controls
{
    [TemplatePart(Name = PART_COMMANDBAR_NAME, Type = typeof(CommandBar))]
    [TemplatePart(Name= PART_BACK_BTN_GRID_NAME, Type = typeof(Grid))]
    [TemplatePart(Name = PART_SPACER_NAME, Type = typeof(Rectangle))]
    [TemplatePart(Name = PART_COMMAND_MAIN_STACK_NAME, Type = typeof(StackPanel))]
    [ContentProperty(Name = nameof(PrimaryCommands))]
    public sealed class PageHeader : Control
    {
        private const string PART_COMMANDBAR_NAME = "PART_COMMANDBAR";
        private const string PART_BACK_BTN_GRID_NAME = "PART_BACK_BTN_GRID";
        private const string PART_SPACER_NAME = "PART_SPACER";
        private const string PART_COMMAND_MAIN_STACK_NAME = "PART_COMMAND_MAIN_STACK";
        private CommandBar _commandBar;
        public PageHeader()
        {
            this.DefaultStyleKey = typeof(PageHeader);
            PrimaryCommands = new ObservableCollection<ICommandBarElement>();
            SecondaryCommands = new ObservableCollection<ICommandBarElement>();
        }

        private void UpdateEllipse()
        {
            if (_commandBar == null)
                return;
            var controls = XamlUtil.AllChildren<Control>(_commandBar);
            var buttons = controls.OfType<Button>();
            var button = buttons.FirstOrDefault(x => x.Name.Equals("MoreButton"));
            if (button != null)
            {
                var count =
                    _commandBar.PrimaryCommands.OfType<Control>().Count(x => x.Visibility.Equals(Visibility.Visible));
                count += _commandBar.SecondaryCommands.OfType<Control>().Count(x => x.Visibility.Equals(Visibility.Visible));
                button.Visibility = (count > 0) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        protected override void OnApplyTemplate()
        {
            _commandBar = GetTemplateChild(PART_COMMANDBAR_NAME) as CommandBar;
            UpdateCommandBar();
        }

        private void UpdateCommandBar()
        {            
            if (PrimaryCommands != null)
            {
                foreach (var command in PrimaryCommands)
                {
                    _commandBar.PrimaryCommands.Add(command);
                }
            }
            if (SecondaryCommands != null)
            {
                foreach (var command in SecondaryCommands)
                {
                    _commandBar.SecondaryCommands.Add(command);
                }
            }
            _commandBar.Loaded += (s, e) => UpdateEllipse();           
        }

        public ObservableCollection<ICommandBarElement> PrimaryCommands
        {
            get { return (ObservableCollection<ICommandBarElement>)GetValue(PrimaryCommandsProperty); }
            set { SetValue(PrimaryCommandsProperty, value); }
        }

        public static readonly DependencyProperty PrimaryCommandsProperty =
            DependencyProperty.Register(nameof(PrimaryCommands), typeof(ObservableCollection<ICommandBarElement>), typeof(PageHeader), new PropertyMetadata(default(ObservableCollection<ICommandBarElement>),
                delegate (DependencyObject o, DependencyPropertyChangedEventArgs args)
                {
                    var pageHeader = o as PageHeader;
                    if (pageHeader?._commandBar == null)
                        return;
                    var newCommands = args.NewValue as ObservableCollection<ICommandBarElement>;
                    var oldCommands = args.OldValue as ObservableCollection<ICommandBarElement>;
                    if (newCommands != null)
                        foreach (var command in newCommands)
                        {
                            pageHeader._commandBar.PrimaryCommands.Add(command);
                        }
                    if (oldCommands != null)
                        foreach (var command in oldCommands)
                        {
                            pageHeader._commandBar.PrimaryCommands.Remove(command);
                        }
                    pageHeader.UpdateEllipse();
                }));

        public ObservableCollection<ICommandBarElement> SecondaryCommands
        {
            get { return (ObservableCollection<ICommandBarElement>)GetValue(SecondaryCommandsProperty); }
            set { SetValue(SecondaryCommandsProperty, value); }
        }

        public static readonly DependencyProperty SecondaryCommandsProperty =
            DependencyProperty.Register(nameof(PrimaryCommands), typeof(ObservableCollection<ICommandBarElement>), typeof(PageHeader), new PropertyMetadata(default(ObservableCollection<ICommandBarElement>),
                delegate (DependencyObject o, DependencyPropertyChangedEventArgs args)
                {
                    var pageHeader = o as PageHeader;
                    if (pageHeader?._commandBar == null)
                        return;
                    var newCommands = args.NewValue as IObservableVector<ICommandBarElement>;
                    var oldCommands = args.OldValue as IObservableVector<ICommandBarElement>;
                    if (newCommands != null)
                        foreach (var command in newCommands)
                        {
                            pageHeader._commandBar.SecondaryCommands.Add(command);
                        }
                    if (oldCommands != null)
                        foreach (var command in oldCommands)
                        {
                            pageHeader._commandBar.SecondaryCommands.Remove(command);
                        }
                    pageHeader.UpdateEllipse();
                }));


        public Visibility BackButtonVisibility
        {
            get { return (Visibility)GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty BackButtonVisibilityProperty =
            DependencyProperty.Register(nameof(BackButtonVisibility), typeof(Visibility), typeof(PageHeader), new PropertyMetadata(default(Visibility)));


        public Frame Frame
        {
            get { return (Frame)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }

        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register(nameof(Frame), typeof(Frame), typeof(PageHeader), new PropertyMetadata(default(Frame)));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(PageHeader), new PropertyMetadata(default(string)));



        public bool IsCommandBarOpen
        {
            get { return _commandBar.IsOpen; }
            set { SetValue(IsCommandBarOpenProperty, _commandBar.IsOpen = value); }
        }


        public static readonly DependencyProperty IsCommandBarOpenProperty =
            DependencyProperty.Register(nameof(IsCommandBarOpen), typeof(bool), typeof(PageHeader), new PropertyMetadata(default(bool)));



        public double VisualStateNarrowMinWidth
        {
            get { return (double)GetValue(VisualStateNarrowMinWidthProperty); }
            set { SetValue(VisualStateNarrowMinWidthProperty, value); }
        }

        public static readonly DependencyProperty VisualStateNarrowMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNarrowMinWidth), typeof(double), typeof(PageHeader), new PropertyMetadata(default(double)));



        public double VisualStateNormalMinWidth
        {
            get { return (double)GetValue(VisualStateNormalMinWidthProperty); }
            set { SetValue(VisualStateNormalMinWidthProperty, value); }
        }

        public static readonly DependencyProperty VisualStateNormalMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNormalMinWidth), typeof(double), typeof(PageHeader), new PropertyMetadata(default(double)));


        public Brush HeaderBackgroundBrush
        {
            get { return (Brush)GetValue(HeaderBackgroundBrushProperty) as Brush; }
            set { SetValue(HeaderBackgroundBrushProperty, value); }
        }

        public static readonly DependencyProperty HeaderBackgroundBrushProperty =
            DependencyProperty.Register(nameof(HeaderBackgroundBrush), typeof(Brush), typeof(PageHeader), new PropertyMetadata(default(Brush)));

        public SolidColorBrush HeaderForegroundBrush
        {
            get { return (SolidColorBrush)GetValue(HeaderForegroundBrushProperty); }
            set { SetValue(HeaderForegroundBrushProperty, value); }
        }

        public static readonly DependencyProperty HeaderForegroundBrushProperty =
            DependencyProperty.Register(nameof(HeaderForegroundBrush), typeof(SolidColorBrush), typeof(PageHeader), new PropertyMetadata(default(SolidColorBrush)));




    }
}
