using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Microsoft.Xaml.Interactivity;
using Template10.Common;

namespace Template10.Behaviors
{
    /// <summary>
    /// The Throttled Event Behavior attaches to a target's event, and executes child actions at a rate no faster than the throttle set (in milliseconds). 
    /// </summary>
    [ContentProperty(Name = "Actions")]
    public sealed class ThrottledEventTriggerBehavior : EventTriggerBehaviorBase
    {
        private readonly EventThrottleHelper throttleHelper;

        public ThrottledEventTriggerBehavior()
        {
            throttleHelper = new EventThrottleHelper();
            throttleHelper.ThrottledEvent += ThrottleHelperOnThrottledEvent;
        }

        private void ThrottleHelperOnThrottledEvent(object sender, object eventArgs)
        {
            Interaction.ExecuteActions(this.ResolvedSource, this.Actions, eventArgs);
        }

        /// <summary>
        /// Throttle period (in milliseconds).
        /// </summary>
        public int Throttle
        {
            get { return (int) GetValue(ThrottleProperty); }
            set { SetValue(ThrottleProperty, value); }
        }

        /// <summary>
        /// Throttle period (in milliseconds)
        /// </summary>
        public static readonly DependencyProperty ThrottleProperty = DependencyProperty.Register("Throttle", typeof (int), typeof (ThrottledEventTriggerBehavior), new PropertyMetadata(1000, ThrottlePropertyChangedCallback));

        private static void ThrottlePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ThrottledEventTriggerBehavior)d;
            obj.throttleHelper.Throttle = (int) e.NewValue;
        }

        /// <summary>
        /// Reset throttle timer after each event.
        /// </summary>
        public bool ResetTimer
        {
            get { return (bool) GetValue(ResetTimerProperty); }
            set { SetValue(ResetTimerProperty, value); }
        }

        /// <summary>
        /// Reset throttle timer after each event.
        /// </summary>
        public static readonly DependencyProperty ResetTimerProperty = DependencyProperty.Register("ResetTimer", typeof (bool), typeof (ThrottledEventTriggerBehavior), new PropertyMetadata(false, ResetTimerPropertyChangedCallback));

        private static void ResetTimerPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ThrottledEventTriggerBehavior)d;
            obj.throttleHelper.ResetTimer = (bool)e.NewValue;
        }

        /// <summary>
        /// Actions to be done on event triggering. In default implementation list of actions is executed immediately.
        /// This is a main class extension point if some additional actions should be done on event trigger.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="eventArgs">Event argument.</param>
        protected override void OnEvent(object sender, object eventArgs)
        {
            throttleHelper.DispatchTriggerEvent(eventArgs);
        }
    }
}