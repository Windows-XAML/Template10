using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10.Common
{
    /// <summary>
    /// Helper class for throttling events.
    /// </summary>
    internal sealed class EventThrottleHelper : DependencyObject
    {
        /// <summary>
        /// Throttled event.
        /// </summary>
        public event EventHandler<object> ThrottledEvent;

        /// <summary>
        /// Throttle time in milliseconds.
        /// </summary>
        public int Throttle
        {
            get { return (int)GetValue(ThrottleProperty); }
            set { SetValue(ThrottleProperty, value); }
        }

        /// <summary>
        /// Reset throttle timer after each event.
        /// </summary>
        public bool ResetTimer
        {
            get { return (bool)GetValue(ResetTimerProperty); }
            set { SetValue(ResetTimerProperty, value); }
        }

        /// <summary>
        /// Reset throttle timer after each event.
        /// </summary>
        public static readonly DependencyProperty ResetTimerProperty = DependencyProperty.Register(nameof(ResetTimer), typeof(bool), typeof(EventThrottleHelper), new PropertyMetadata(false));


        /// <summary>
        /// Throttle time in milliseconds.
        /// </summary>
        public static readonly DependencyProperty ThrottleProperty = DependencyProperty.Register(nameof(Throttle), typeof(int), typeof(EventThrottleHelper), new PropertyMetadata(1000));

        /// <summary>
        /// Trigger a throttled event. Can only be called from the UI thread.
        /// </summary>
        /// <param name="eventData">Event data (only last event data will be transfered to event handler).</param>
        public async void TriggerEvent(object eventData)
        {
            // TODO: (Jerry) dispatch?
            await DelayAction(eventData).ConfigureAwait(false);
        }

        /// <summary>
        /// Trigger a throttled event inside a dispatcher.
        /// </summary>
        /// <param name="eventData">Event data (only last event data will be transfered to event handler).</param>
        public async void DispatchTriggerEvent(object eventData)
        {
            // TODO: (Jerry) dispatch?
            await DelayAction(eventData).ConfigureAwait(false);
        }

        private bool isWaiting;

        private bool isRefreshed;

        private DateTime stamp;

        private object savedEventData;

        private async Task DelayAction(object eventData)
        {
            Interlocked.Exchange(ref savedEventData, eventData);
            if (ResetTimer || !isWaiting)
            {
                stamp = DateTime.Now;
                isRefreshed = true;
            }
            if (isWaiting)
            {
                return;
            }
            isWaiting = true;
            try
            {
                while (isRefreshed)
                {
                    isRefreshed = false;
                    var toWait = (stamp.AddMilliseconds(Throttle) - DateTime.Now);
                    if (toWait.Ticks > 0)
                    {
                        await Task.Delay(toWait).ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                isWaiting = false;
            }
            var callEventData = Interlocked.Exchange(ref savedEventData, null);
            ThrottledEvent?.Invoke(this, callEventData);
        }
    }
}