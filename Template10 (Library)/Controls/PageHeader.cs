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
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Template10.Controls
{
    [TemplatePart(Name = PART_COMMANDBAR_NAME, Type = typeof(CommandBar))]
    public sealed class PageHeader : Control
    {
        private const string PART_COMMANDBAR_NAME = "PART_COMMANDBAR";
        protected CommandBar _commandBar;
        public PageHeader()
        {
            this.DefaultStyleKey = typeof(PageHeader);
            PrimaryCommands = new ObservableCollection<ICommandBarElement>();
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



        public int VisualStateNarrowMinWidth
        {
            get { return (int)GetValue(VisualStateNarrowMinWidthProperty); }
            set { SetValue(VisualStateNarrowMinWidthProperty, value); }
        }

        public static readonly DependencyProperty VisualStateNarrowMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNarrowMinWidth), typeof(int), typeof(PageHeader), new PropertyMetadata(default(int)));



        public int VisualStateNormalMinWidth
        {
            get { return (int)GetValue(VisualStateNormalMinWidthProperty); }
            set { SetValue(VisualStateNormalMinWidthProperty, value); }
        }

        public static readonly DependencyProperty VisualStateNormalMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNormalMinWidth), typeof(int), typeof(PageHeader), new PropertyMetadata(default(int)));



        public SolidColorBrush HeaderBackgroundBrush
        {
            get { return (SolidColorBrush)GetValue(HeaderBackgroundBrushProperty); }
            set { SetValue(HeaderBackgroundBrushProperty, value); }
        }

        public static readonly DependencyProperty HeaderBackgroundBrushProperty =
            DependencyProperty.Register(nameof(HeaderBackgroundBrush), typeof(SolidColorBrush), typeof(PageHeader), new PropertyMetadata(default(SolidColorBrush)));



        public SolidColorBrush HeaderForegroundBrush
        {
            get { return (SolidColorBrush)GetValue(HeaderForegroundBrushProperty); }
            set { SetValue(HeaderForegroundBrushProperty, value); }
        }

        public static readonly DependencyProperty HeaderForegroundBrushProperty =
            DependencyProperty.Register(nameof(HeaderForegroundBrush), typeof(SolidColorBrush), typeof(PageHeader), new PropertyMetadata(default(SolidColorBrush)));




    }
}
