﻿using System.Collections.ObjectModel;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Template10.Controls
{
    /// <summary>
    ///     Represents a control that displays data items in a vertical stack, with accompanying selected data item in more
    ///     detail. Supports separate commandbars for master and details views, which move to the bottom in the case of
    ///     mobile devices for easier access
    /// </summary>
    [TemplatePart(Name = nameof(MasterCommandBarElement), Type = typeof (CommandBar))]
    [TemplatePart(Name = nameof(MobileMasterCommandBarElement), Type = typeof (CommandBar))]
    [TemplatePart(Name = nameof(MasterProgressBarElement), Type = typeof (ProgressBar))]
    [TemplatePart(Name = nameof(DetailsCommandBarElement), Type = typeof (CommandBar))]
    [TemplatePart(Name = nameof(MobileDetailsCommandBarElement), Type = typeof (CommandBar))]
    [TemplatePart(Name = nameof(DetailsProgressRingElement), Type = typeof (ProgressRing))]
    [TemplateVisualState(Name = MasterVisualStateName, GroupName = NarrowVisualStateGroupName)]
    [TemplateVisualState(Name = DetailsVisualStateName, GroupName = NarrowVisualStateGroupName)]
    [TemplateVisualState(Name = NarrowVisualStateName, GroupName = AdaptiveVisualStateGroupName)]
    [TemplateVisualState(Name = NormalVisualStateName, GroupName = AdaptiveVisualStateGroupName)]
    [ContentProperty(Name = nameof(Items))]
    public sealed class MasterDetailsView : ListView
    {
        private static readonly bool IsMobile = AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";

        #region Visual States

        public const string AdaptiveVisualStateGroupName = "AdaptiveVisualStateGroup";
        public const string NarrowVisualStateGroupName = "NarrowVisualStateGroup";
        public const string MasterVisualStateName = "MasterVisualState";
        public const string DetailsVisualStateName = "DetailsVisualState";
        public const string NarrowVisualStateName = "VisualStateNarrow";
        public const string NormalVisualStateName = "VisualStateNormal";

        private VisualStateGroup AdaptiveVisualStateGroupElement { get; set; }
        private VisualState NarrowVisualStateElement { get; set; }
        private VisualState NormalVisualStateElement { get; set; }
        private VisualStateGroup NarrowVisualStateGroupElement { get; set; }
        private VisualState MasterVisualStateElement { get; set; }
        private VisualState DetailsVisualStateElement { get; set; }

        #endregion
        public MasterDetailsView()
        {
            DefaultStyleKey = typeof (MasterDetailsView);
            MasterCommands = new ObservableCollection<ICommandBarElement>();
            DetailsCommands = new ObservableCollection<ICommandBarElement>();
            ItemClick += OnItemClick;
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            DetailsRequested = false;
            DetailsRequested = true;
        }

        protected override void OnApplyTemplate()
        {
            // Visual States
            AdaptiveVisualStateGroupElement = GetTemplateChild(AdaptiveVisualStateGroupName) as VisualStateGroup;
            NarrowVisualStateElement = GetTemplateChild(NarrowVisualStateName) as VisualState;
            NormalVisualStateElement = GetTemplateChild(NormalVisualStateName) as VisualState;
            NarrowVisualStateGroupElement = GetTemplateChild(NarrowVisualStateGroupName) as VisualStateGroup;
            MasterVisualStateElement = GetTemplateChild(MasterVisualStateName) as VisualState;
            DetailsVisualStateElement = GetTemplateChild(DetailsVisualStateName) as VisualState;

            // Master
            MasterProgressBarElement = GetTemplateChild("MasterProgressBar") as ProgressBar;
            MasterCommandBarElement = GetTemplateChild("MasterCommandBar") as CommandBar;
            MobileMasterCommandBarElement = GetTemplateChild("MobileMasterCommandBar") as CommandBar;
            if (ActiveMasterCommandBar != null && MasterCommands != null)
            {
                ActiveMasterCommandBar.PrimaryCommands.Clear();
                var commands = ActiveMasterCommandBar.PrimaryCommands;
                foreach (var command in MasterCommands)
                {
                    commands.Add(command);
                }
            }

            // Details
            DetailsProgressRingElement = GetTemplateChild("DetailsProgressRing") as ProgressRing;
            DetailsCommandBarElement = GetTemplateChild("DetailsCommandBar") as CommandBar;
            MobileDetailsCommandBarElement = GetTemplateChild("MobileDetailsCommandBar") as CommandBar;
            if (ActiveDetailsCommandBar != null && DetailsCommands != null)
            {
                ActiveDetailsCommandBar.PrimaryCommands.Clear();
                foreach (var command in DetailsCommands)
                {
                    ActiveDetailsCommandBar.PrimaryCommands.Add(command);
                }
            }
        }

        public static readonly DependencyProperty NormalMinWidthProperty = DependencyProperty.Register(
            nameof(NormalMinWidth), typeof (double), typeof (MasterDetailsView), new PropertyMetadata(default(double)));

        public double NormalMinWidth
        {
            get { return (double) GetValue(NormalMinWidthProperty); }
            set { SetValue(NormalMinWidthProperty, value); }
        }

        #region Master

        public static readonly DependencyProperty IsMasterLoadingProperty = DependencyProperty.Register(
            nameof(IsMasterLoading), typeof (bool), typeof (MasterDetailsView), new PropertyMetadata(default(bool),
                (sender, args) =>
                {
                    var control = sender as MasterDetailsView;
                    if (control == null) return;
                    var newValue = (bool) args.NewValue;
                    var visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
                    if (control.MasterProgressBarElement != null)
                        control.MasterProgressBarElement.Visibility = visibility;
                }));

        public bool IsMasterLoading
        {
            get { return (bool) GetValue(IsMasterLoadingProperty); }
            set { SetValue(IsMasterLoadingProperty, value); }
        }

        public ProgressBar MasterProgressBarElement { get; set; }

        #region CommandBars

        public CommandBar MasterCommandBarElement { get; set; }
        public CommandBar MobileMasterCommandBarElement { get; set; }

        private CommandBar ActiveMasterCommandBar => IsMobile ? MobileMasterCommandBarElement : MasterCommandBarElement;

        public static readonly DependencyProperty MasterCommandBarContentProperty = DependencyProperty.Register(
            nameof(MasterCommandBarContent), typeof (object), typeof (MasterDetailsView),
            new PropertyMetadata(default(object)));

        public object MasterCommandBarContent
        {
            get { return GetValue(MasterCommandBarContentProperty); }
            set { SetValue(MasterCommandBarContentProperty, value); }
        }

        public static readonly DependencyProperty MasterCommandsProperty = DependencyProperty.Register(
            nameof(MasterCommands), typeof (ObservableCollection<ICommandBarElement>), typeof (MasterDetailsView),
            new PropertyMetadata(default(ObservableCollection<ICommandBarElement>)));

        public ObservableCollection<ICommandBarElement> MasterCommands
        {
            get { return (ObservableCollection<ICommandBarElement>) GetValue(MasterCommandsProperty); }
            set { SetValue(MasterCommandsProperty, value); }
        }

        #endregion

        #endregion

        #region Details

        public static readonly DependencyProperty IsDetailsLoadingProperty = DependencyProperty.Register(
            nameof(IsDetailsLoading), typeof (bool), typeof (MasterDetailsView), new PropertyMetadata(default(bool),
                (sender, args) =>
                {
                    var control = sender as MasterDetailsView;
                    if (control == null) return;
                    var newValue = (bool) args.NewValue;
                    var visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
                    if (control.DetailsProgressRingElement != null)
                        control.DetailsProgressRingElement.Visibility = visibility;
                }));

        public bool IsDetailsLoading
        {
            get { return (bool) GetValue(IsDetailsLoadingProperty); }
            set { SetValue(IsDetailsLoadingProperty, value); }
        }

        public static readonly DependencyProperty DetailsRequestedProperty = DependencyProperty.Register(
            nameof(DetailsRequested), typeof (bool), typeof (MasterDetailsView), new PropertyMetadata(default(bool),
                (sender, args) =>
                {
                    var control = sender as MasterDetailsView;
                    if (control == null)
                        return;
                    if (!control.DetailsRequested)
                    {
                        if (control.MasterVisualStateElement != null)
                            VisualStateManager.GoToState(control, MasterVisualStateName, true);
                        return;
                    }
                    if (control.AdaptiveVisualStateGroupElement?.CurrentState?.Name == NarrowVisualStateName &&
                        control.DetailsVisualStateElement != null)
                        VisualStateManager.GoToState(control, DetailsVisualStateName, true);
                }));

        public bool DetailsRequested
        {
            get { return (bool) GetValue(DetailsRequestedProperty); }
            set { SetValue(DetailsRequestedProperty, value); }
        }

        public ProgressRing DetailsProgressRingElement { get; set; }

        public static readonly DependencyProperty DetailsTemplateProperty = DependencyProperty.Register(
            nameof(DetailsTemplate), typeof (DataTemplate), typeof (MasterDetailsView),
            new PropertyMetadata(default(DataTemplate)));

        public DataTemplate DetailsTemplate
        {
            get { return (DataTemplate) GetValue(DetailsTemplateProperty); }
            set { SetValue(DetailsTemplateProperty, value); }
        }

        public static readonly DependencyProperty DetailsProperty = DependencyProperty.Register(
            nameof(Details), typeof (object), typeof (MasterDetailsView), new PropertyMetadata(default(object)));

        public object Details
        {
            get { return GetValue(DetailsProperty); }
            set { SetValue(DetailsProperty, value); }
        }

        #region CommandBars

        public CommandBar DetailsCommandBarElement { get; set; }
        public CommandBar MobileDetailsCommandBarElement { get; set; }

        private CommandBar ActiveDetailsCommandBar
            => IsMobile ? MobileDetailsCommandBarElement : DetailsCommandBarElement;

        public static readonly DependencyProperty DetailsCommandBarContentProperty = DependencyProperty.Register(
            nameof(DetailsCommandBarContent), typeof (object), typeof (MasterDetailsView),
            new PropertyMetadata(default(object)));

        public object DetailsCommandBarContent
        {
            get { return GetValue(DetailsCommandBarContentProperty); }
            set { SetValue(DetailsCommandBarContentProperty, value); }
        }

        public static readonly DependencyProperty DetailsCommandsProperty = DependencyProperty.Register(
            nameof(DetailsCommands), typeof (ObservableCollection<ICommandBarElement>), typeof (MasterDetailsView),
            new PropertyMetadata(default(ObservableCollection<ICommandBarElement>)));

        public ObservableCollection<ICommandBarElement> DetailsCommands
        {
            get { return (ObservableCollection<ICommandBarElement>) GetValue(DetailsCommandsProperty); }
            set { SetValue(DetailsCommandsProperty, value); }
        }

        #endregion

        #endregion
    }
}