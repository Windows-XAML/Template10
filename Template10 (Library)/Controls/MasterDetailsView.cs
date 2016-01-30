using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Template10.Common;
using Template10.Utils;
using Template10.Mvvm;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Template10.Controls
{
    // From https://msdn.microsoft.com/en-us/library/cc278064(VS.95).aspx

    /// <summary>
    ///     Represents a control that displays data items in a vertical stack, with accompanying selected data item in more
    ///     detail. Supports separate commandbars for master and details views, which move to the bottom in the case of
    ///     mobile devices for easier access
    /// </summary>
    [TemplatePart(Name = MasterCommandBarName, Type = typeof (CommandBar))]
    [TemplatePart(Name = MobileMasterCommandBarName, Type = typeof (CommandBar))]
    [TemplatePart(Name = MasterProgressBarName, Type = typeof (ProgressBar))]
    [TemplatePart(Name = DetailsCommandBarName, Type = typeof (CommandBar))]
    [TemplatePart(Name = MobileDetailsCommandBarName, Type = typeof (CommandBar))]
    [TemplatePart(Name = DetailsProgressRingName, Type = typeof (ProgressRing))]
    [TemplateVisualState(Name = MasterVisualStateName, GroupName = NarrowVisualStateGroupName)]
    [TemplateVisualState(Name = DetailsVisualStateName, GroupName = NarrowVisualStateGroupName)]
    [TemplateVisualState(Name = NarrowVisualStateName, GroupName = AdaptiveVisualStateGroupName)]
    [TemplateVisualState(Name = NormalVisualStateName, GroupName = AdaptiveVisualStateGroupName)]
    [ContentProperty(Name = nameof(Items))]
    public sealed class MasterDetailsView : ListView, IBindable
    {
        private static readonly bool IsMobile = AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";

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
            MasterProgressBar = GetTemplateChild(MasterProgressBarName) as ProgressBar;
            MasterCommandBar = GetTemplateChild(MasterCommandBarName) as CommandBar;
            MobileMasterCommandBar = GetTemplateChild(MobileMasterCommandBarName) as CommandBar;
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
            DetailsProgressRing = GetTemplateChild(DetailsProgressRingName) as ProgressRing;
            DetailsCommandBar = GetTemplateChild(DetailsCommandBarName) as CommandBar;
            MobileDetailsCommandBar = GetTemplateChild(MobileDetailsCommandBarName) as CommandBar;
            if (ActiveDetailsCommandBar != null && DetailsCommands != null)
            {
                ActiveDetailsCommandBar.PrimaryCommands.Clear();
                foreach (var command in DetailsCommands)
                {
                    ActiveDetailsCommandBar.PrimaryCommands.Add(command);
                }
            }
        }

        #region Control Contract Properties

        public const string MasterProgressBarName = "MasterProgressBar";
        public const string MasterCommandBarName = "MasterCommandBar";
        public const string MobileMasterCommandBarName = "MobileMasterCommandBar";
        public const string DetailsProgressRingName = "DetailsProgressRing";
        public const string DetailsCommandBarName = "DetailsCommandBar";
        public const string MobileDetailsCommandBarName = "MobileDetailsCommandBar";

        public const string AdaptiveVisualStateGroupName = "AdaptiveVisualStateGroup";
        public const string NarrowVisualStateGroupName = "NarrowVisualStateGroup";
        public const string MasterVisualStateName = "MasterVisualState";
        public const string DetailsVisualStateName = "DetailsVisualState";
        public const string NarrowVisualStateName = "VisualStateNarrow";
        public const string NormalVisualStateName = "VisualStateNormal";

        #endregion

        #region Visual States

        private VisualStateGroup AdaptiveVisualStateGroupElement { get; set; }
        private VisualState NarrowVisualStateElement { get; set; }
        private VisualState NormalVisualStateElement { get; set; }
        private VisualStateGroup NarrowVisualStateGroupElement { get; set; }
        private VisualState MasterVisualStateElement { get; set; }
        private VisualState DetailsVisualStateElement { get; set; }

        public static readonly DependencyProperty VisualStateNarrowMinWidthProperty = DependencyProperty.Register(
            nameof(VisualStateNarrowMinWidth), typeof (double), typeof (MasterDetailsView),
            new PropertyMetadata(default(double)));

        public double VisualStateNarrowMinWidth
        {
            get { return (double) GetValue(VisualStateNarrowMinWidthProperty); }
            set { SetValue(VisualStateNarrowMinWidthProperty, value); }
        }

        public static readonly DependencyProperty VisualStateNormalMinWidthProperty = DependencyProperty.Register(
            nameof(VisualStateNormalMinWidth), typeof (double), typeof (MasterDetailsView),
            new PropertyMetadata(default(double)));

        public double VisualStateNormalMinWidth
        {
            get { return (double) GetValue(VisualStateNormalMinWidthProperty); }
            set { SetValue(VisualStateNormalMinWidthProperty, value); }
        }

        #endregion

        #region Master

        public static readonly DependencyProperty IsMasterLoadingProperty = DependencyProperty.Register(
            nameof(IsMasterLoading), typeof (bool), typeof (MasterDetailsView), new PropertyMetadata(default(bool),
                (sender, args) =>
                {
                    var control = sender as MasterDetailsView;
                    if (control == null) return;
                    var newValue = (bool) args.NewValue;
                    var visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
                    if (control.MasterProgressBar != null)
                        control.MasterProgressBar.Visibility = visibility;
                }));

        public bool IsMasterLoading
        {
            get { return (bool) GetValue(IsMasterLoadingProperty); }
            set { SetValue(IsMasterLoadingProperty, value); }
        }

        public ProgressBar MasterProgressBar { get; set; }

        public static readonly DependencyProperty MasterPaneWidthProperty = DependencyProperty.Register(
            nameof(MasterPaneWidth), typeof (double), typeof (MasterDetailsView), new PropertyMetadata(default(double)));

        public double MasterPaneWidth
        {
            get { return (double) GetValue(MasterPaneWidthProperty); }
            set { SetValue(MasterPaneWidthProperty, value); }
        }

        #region CommandBars

        public CommandBar MasterCommandBar { get; set; }
        public CommandBar MobileMasterCommandBar { get; set; }

        private CommandBar ActiveMasterCommandBar => IsMobile ? MobileMasterCommandBar : MasterCommandBar;

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
                    if (control.DetailsProgressRing != null)
                        control.DetailsProgressRing.Visibility = visibility;
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
                    if (control == null || control.AdaptiveVisualStateGroupElement?.CurrentState?.Name != NarrowVisualStateName)
                        return;
                    if (!control.DetailsRequested)
                    {
                        if (control.MasterVisualStateElement != null)
                            VisualStateManager.GoToState(control, MasterVisualStateName, true);
                        return;
                    }
                    if (control.DetailsVisualStateElement != null)
                        VisualStateManager.GoToState(control, DetailsVisualStateName, true);
                }));

        public bool DetailsRequested
        {
            get { return (bool) GetValue(DetailsRequestedProperty); }
            set { SetValue(DetailsRequestedProperty, value); }
        }

        public ProgressRing DetailsProgressRing { get; set; }

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

        public CommandBar DetailsCommandBar { get; set; }
        public CommandBar MobileDetailsCommandBar { get; set; }

        private CommandBar ActiveDetailsCommandBar
            => IsMobile ? MobileDetailsCommandBar : DetailsCommandBar;

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

        #region IBindable

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (DesignMode.DesignModeEnabled)
                return;

            var handler = PropertyChanged;
            //if is not null
            if (Equals(handler, null)) return;
            var args = new PropertyChangedEventArgs(propertyName);
            try
            {
                handler.Invoke(this, args);
            }
            catch
            {
                WindowWrapper.Current().Dispatcher.Dispatch(() => handler.Invoke(this, args));
            }
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        public bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
        {
            //if is equal 
            if (Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(propertyExpression);
            return true;
        }

        public void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (DesignMode.DesignModeEnabled)
                return;

            var handler = PropertyChanged;
            //if is not null
            if (Equals(handler, null)) return;
            var propertyName = ExpressionUtils.GetPropertyName(propertyExpression);

            if (Equals(propertyName, null)) return;
            var args = new PropertyChangedEventArgs(propertyName);
            try
            {
                handler.Invoke(this, args);
            }
            catch
            {
                WindowWrapper.Current().Dispatcher.Dispatch(() => handler.Invoke(this, args));
            }
        }

        #endregion
    }
}