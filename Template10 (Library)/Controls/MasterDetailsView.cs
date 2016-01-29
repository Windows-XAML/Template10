using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Template10.Controls
{
    /// <summary>
    /// Represents a control that displays data items in a vertical stack, with accompanying selected data item in more detail
    /// </summary>
    [TemplatePart(Name = nameof(MasterCommandBarElement), Type = typeof(CommandBar))]
    [TemplatePart(Name = nameof(MobileMasterCommandBarElement), Type = typeof(CommandBar))]
    [TemplatePart(Name = nameof(DetailsCommandBarElement), Type = typeof(CommandBar))]
    [TemplatePart(Name = nameof(MobileDetailsCommandBarElement), Type = typeof(CommandBar))]
    [TemplatePart(Name = nameof(DetailsProgressRingElement), Type = typeof(ProgressRing))]
    [ContentProperty(Name = nameof(Items))]
    public sealed class MasterDetailsView : ListView
    {
        private static readonly bool _isMobile = AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";
        public MasterDetailsView()
        {
            this.DefaultStyleKey = typeof(MasterDetailsView);
            MasterCommands = new ObservableCollection<ICommandBarElement>();
            DetailsCommands = new ObservableCollection<ICommandBarElement>();
        }

        #region Master

        public static readonly DependencyProperty IsMasterLoadingProperty = DependencyProperty.Register(
            nameof(IsMasterLoading), typeof(bool), typeof(MasterDetailsView), new PropertyMetadata(default(bool)));

        public bool IsMasterLoading
        {
            get { return (bool)GetValue(IsMasterLoadingProperty); }
            set { SetValue(IsMasterLoadingProperty, value); }
        }

        #region CommandBars

        public CommandBar MasterCommandBarElement { get; set; }
        public CommandBar MobileMasterCommandBarElement { get; set; }

        private CommandBar ActiveMasterCommandBar => _isMobile ? MobileMasterCommandBarElement : MasterCommandBarElement;

        public static readonly DependencyProperty MasterCommandBarContentProperty = DependencyProperty.Register(
            nameof(MasterCommandBarContent), typeof(object), typeof(MasterDetailsView), new PropertyMetadata(default(object)));

        public object MasterCommandBarContent
        {
            get { return (object)GetValue(MasterCommandBarContentProperty); }
            set { SetValue(MasterCommandBarContentProperty, value); }
        }

        public static readonly DependencyProperty MasterCommandsProperty = DependencyProperty.Register(
            nameof(MasterCommands), typeof(ObservableCollection<ICommandBarElement>), typeof(MasterDetailsView), new PropertyMetadata(default(ObservableCollection<ICommandBarElement>)));

        public ObservableCollection<ICommandBarElement> MasterCommands
        {
            get { return (ObservableCollection<ICommandBarElement>)GetValue(MasterCommandsProperty); }
            set { SetValue(MasterCommandsProperty, value); }
        }

        #endregion

        #endregion

        #region Details

        public static readonly DependencyProperty IsDetailsLoadingProperty = DependencyProperty.Register(
            nameof(IsDetailsLoading), typeof(bool), typeof(MasterDetailsView), new PropertyMetadata(default(bool),
                (sender, args) =>
                {
                    var control = sender as MasterDetailsView;
                    if (control == null) return;
                    var newValue = (bool)args.NewValue;
                    var visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
                    if (control.DetailsProgressRingElement != null)
                        control.DetailsProgressRingElement.Visibility = visibility;
                }));

        public bool IsDetailsLoading
        {
            get { return (bool)GetValue(IsDetailsLoadingProperty); }
            set { SetValue(IsDetailsLoadingProperty, value); }
        }

        public ProgressRing DetailsProgressRingElement { get; set; }

        public static readonly DependencyProperty DetailsTemplateProperty = DependencyProperty.Register(
            nameof(DetailsTemplate), typeof(DataTemplate), typeof(MasterDetailsView), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate DetailsTemplate
        {
            get { return (DataTemplate)GetValue(DetailsTemplateProperty); }
            set { SetValue(DetailsTemplateProperty, value); }
        }

        public static readonly DependencyProperty DetailsProperty = DependencyProperty.Register(
            nameof(Details), typeof(object), typeof(MasterDetailsView), new PropertyMetadata(default(object)));

        public object Details
        {
            get { return (object)GetValue(DetailsProperty); }
            set { SetValue(DetailsProperty, value); }
        }

        #region CommandBars

        public CommandBar DetailsCommandBarElement { get; set; }
        public CommandBar MobileDetailsCommandBarElement { get; set; }

        private CommandBar ActiveDetailsCommandBar => _isMobile ? MobileDetailsCommandBarElement : DetailsCommandBarElement;

        public static readonly DependencyProperty DetailsCommandBarContentProperty = DependencyProperty.Register(
            nameof(DetailsCommandBarContent), typeof(object), typeof(MasterDetailsView), new PropertyMetadata(default(object)));

        public object DetailsCommandBarContent
        {
            get { return (object)GetValue(DetailsCommandBarContentProperty); }
            set { SetValue(DetailsCommandBarContentProperty, value); }
        }

        public static readonly DependencyProperty DetailsCommandsProperty = DependencyProperty.Register(
            nameof(DetailsCommands), typeof(ObservableCollection<ICommandBarElement>), typeof(MasterDetailsView), new PropertyMetadata(default(ObservableCollection<ICommandBarElement>)));

        public ObservableCollection<ICommandBarElement> DetailsCommands
        {
            get { return (ObservableCollection<ICommandBarElement>)GetValue(DetailsCommandsProperty); }
            set { SetValue(DetailsCommandsProperty, value); }
        }

        #endregion

        #endregion

        protected override void OnApplyTemplate()
        {
            // Master
            MasterCommandBarElement = GetTemplateChild("MasterCommandBar") as CommandBar;
            MobileMasterCommandBarElement = GetTemplateChild("MobileMasterCommandBar") as CommandBar;
            if (ActiveMasterCommandBar != null && MasterCommands != null)
            {
                ActiveMasterCommandBar.PrimaryCommands.Clear();
                foreach (var command in MasterCommands)
                {
                    ActiveMasterCommandBar.PrimaryCommands.Add(command);
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
    }
}
