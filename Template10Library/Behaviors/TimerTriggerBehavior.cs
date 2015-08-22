using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-XamlBehaviors
    [ContentProperty(Name = nameof(Actions))]
    public sealed class TimerTriggerBehavior : DependencyObject, IBehavior
    {
        private int _tickCount;
        private DispatcherTimer _timer;

        public DependencyObject AssociatedObject { get; private set; }
        public void Attach(DependencyObject associatedObject)
        {
            if ((associatedObject != AssociatedObject) && !Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                if (AssociatedObject != null)
                    throw new InvalidOperationException("Cannot attach behavior multiple times.");
                AssociatedObject = associatedObject;
                StartTimer();
            }
        }

        public void Detach()
        {
            this.StopTimer();
        }

        public ActionCollection Actions
        {
            get
            {
                ActionCollection actions = (ActionCollection)base.GetValue(ActionsProperty);
                if (actions == null)
                {
                    actions = new ActionCollection();
                    base.SetValue(ActionsProperty, actions);
                }
                return actions;
            }
        }
        public static readonly DependencyProperty ActionsProperty =
            DependencyProperty.Register("Actions", typeof(ActionCollection),
                typeof(TimerTriggerBehavior), new PropertyMetadata(null));

        public double MillisecondsPerTick
        {
            get
            {
                return (double)base.GetValue(MillisecondsPerTickProperty);
            }
            set
            {
                base.SetValue(MillisecondsPerTickProperty, value);
            }
        }
        public static readonly DependencyProperty MillisecondsPerTickProperty =
            DependencyProperty.Register("MillisecondsPerTick", typeof(double),
                typeof(TimerTriggerBehavior), new PropertyMetadata(1000.0));

        public int TotalTicks
        {
            get
            {
                return (int)base.GetValue(TotalTicksProperty);
            }
            set
            {
                base.SetValue(TotalTicksProperty, value);
            }
        }
        public static readonly DependencyProperty TotalTicksProperty =
            DependencyProperty.Register("TotalTicks", typeof(int),
                typeof(TimerTriggerBehavior), new PropertyMetadata(-1));

        private void OnTimerTick(object sender, object e)
        {
            if (this.TotalTicks > 0 && ++this._tickCount >= this.TotalTicks)
            {
                this.StopTimer();
            }

            // Raise the actions
            Interaction.ExecuteActions(AssociatedObject, this.Actions, null);
        }

        internal void StartTimer()
        {
            this._timer = new DispatcherTimer { Interval = (TimeSpan.FromMilliseconds(this.MillisecondsPerTick)) };
            this._timer.Tick += this.OnTimerTick;
            this._timer.Start();
        }

        internal void StopTimer()
        {
            this._timer.Stop();
        }
    }
}
