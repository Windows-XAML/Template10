using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    [ContentProperty(Name = nameof(Actions))]
    public class DeviceDispositionBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            Utils.DeviceUtils.Current().Changed += DeviceDispositionBehavior_Changed;
            Update();
        }

        public void Detach() => Utils.DeviceUtils.Current().Changed -= DeviceDispositionBehavior_Changed;
        private void DeviceDispositionBehavior_Changed(object sender, EventArgs e) => Update();
        private void Run() => Interaction.ExecuteActions(AssociatedObject, Actions, null);

        private void Update()
        {
            switch (Utils.DeviceUtils.Current().DeviceDisposition())
            {
                case Utils.DeviceUtils.DeviceDispositions.IoT: if (IoT) Run(); break;
                case Utils.DeviceUtils.DeviceDispositions.Xbox: if (Xbox) Run(); break;
                case Utils.DeviceUtils.DeviceDispositions.Team: if (Team) Run(); break;
                case Utils.DeviceUtils.DeviceDispositions.HoloLens: if (HoloLens) Run(); break;
                case Utils.DeviceUtils.DeviceDispositions.Desktop: if (Desktop) Run(); break;
                case Utils.DeviceUtils.DeviceDispositions.Mobile: if (Mobile) Run(); break;
                case Utils.DeviceUtils.DeviceDispositions.Phone: if (Phone) Run(); break;
                case Utils.DeviceUtils.DeviceDispositions.Continuum: if (Continuum) Run(); break;
                case Utils.DeviceUtils.DeviceDispositions.Virtual: if (Virtual) Run(); break;
            }
        }

        public ActionCollection Actions
        {
            get
            {
                var actions = (ActionCollection)base.GetValue(ActionsProperty);
                if (actions == null)
                {
                    SetValue(ActionsProperty, actions = new ActionCollection());
                }
                return actions;
            }
        }
        public static readonly DependencyProperty ActionsProperty =
            DependencyProperty.Register(nameof(Actions), typeof(ActionCollection),
                typeof(DeviceDispositionBehavior), new PropertyMetadata(null));

        public bool IoT { get { return (bool)GetValue(IoTProperty); } set { SetValue(IoTProperty, value); } }
        public static readonly DependencyProperty IoTProperty =
            DependencyProperty.Register(nameof(IoT), typeof(bool),
                typeof(DeviceDispositionBehavior), new PropertyMetadata(false));

        public bool Xbox { get { return (bool)GetValue(XboxProperty); } set { SetValue(XboxProperty, value); } }
        public static readonly DependencyProperty XboxProperty =
            DependencyProperty.Register(nameof(Xbox), typeof(bool),
                typeof(DeviceDispositionBehavior), new PropertyMetadata(false));

        public bool Team { get { return (bool)GetValue(TeamProperty); } set { SetValue(TeamProperty, value); } }
        public static readonly DependencyProperty TeamProperty =
            DependencyProperty.Register(nameof(Team), typeof(bool),
                typeof(DeviceDispositionBehavior), new PropertyMetadata(false));

        public bool HoloLens { get { return (bool)GetValue(HoloLensProperty); } set { SetValue(HoloLensProperty, value); } }
        public static readonly DependencyProperty HoloLensProperty =
            DependencyProperty.Register(nameof(HoloLens), typeof(bool),
                typeof(DeviceDispositionBehavior), new PropertyMetadata(false));

        public bool Desktop { get { return (bool)GetValue(DesktopProperty); } set { SetValue(DesktopProperty, value); } }
        public static readonly DependencyProperty DesktopProperty =
            DependencyProperty.Register(nameof(Desktop), typeof(bool),
                typeof(DeviceDispositionBehavior), new PropertyMetadata(false));

        public bool Mobile { get { return (bool)GetValue(MobileProperty); } set { SetValue(MobileProperty, value); } }
        public static readonly DependencyProperty MobileProperty =
            DependencyProperty.Register(nameof(Mobile), typeof(bool),
                typeof(DeviceDispositionBehavior), new PropertyMetadata(false));

        public bool Phone { get { return (bool)GetValue(PhoneProperty); } set { SetValue(PhoneProperty, value); } }
        public static readonly DependencyProperty PhoneProperty =
            DependencyProperty.Register(nameof(Phone), typeof(bool),
                typeof(DeviceDispositionBehavior), new PropertyMetadata(false));

        public bool Continuum { get { return (bool)GetValue(ContinuumProperty); } set { SetValue(ContinuumProperty, value); } }
        public static readonly DependencyProperty ContinuumProperty =
            DependencyProperty.Register(nameof(Continuum), typeof(bool),
                typeof(DeviceDispositionBehavior), new PropertyMetadata(false));

        public bool Virtual { get { return (bool)GetValue(VirtualProperty); } set { SetValue(VirtualProperty, value); } }
        public static readonly DependencyProperty VirtualProperty =
            DependencyProperty.Register(nameof(Virtual), typeof(bool),
                typeof(DeviceDispositionBehavior), new PropertyMetadata(false));

    }
}
