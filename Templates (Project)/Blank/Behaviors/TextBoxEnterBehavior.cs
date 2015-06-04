using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    /// <summary>
    /// Trigger which runs off a timer
    /// </summary>
    [ContentProperty(Name = "Actions")]
    public sealed class TimerTriggerBehavior : DependencyObject, IBehavior
    {
        private int _tickCount;
        private DispatcherTimer _timer;

        /// <summary>
        /// Backing storage for Actions collection
        /// </summary>
        public static readonly DependencyProperty ActionsProperty =
            DependencyProperty.Register("Actions", typeof(ActionCollection), typeof(TimerTriggerBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Actions collection
        /// </summary>
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

        /// <summary>
        /// Backing storage for the counter
        /// </summary>
        public static readonly DependencyProperty MillisecondsPerTickProperty =
            DependencyProperty.Register("MillisecondsPerTick", typeof(double), typeof(TimerTriggerBehavior), new PropertyMetadata(1000.0));

        /// <summary>
        /// Milliseconds
        /// </summary>
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

        /// <summary>
        /// Backing storage for the total ticks counter
        /// </summary>
        public static readonly DependencyProperty TotalTicksProperty =
            DependencyProperty.Register("TotalTicks", typeof(int), typeof(TimerTriggerBehavior), new PropertyMetadata(-1));

        /// <summary>
        /// Total ticks elapsed
        /// </summary>
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

        /// <summary>
        /// Called when the timer elapses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerTick(object sender, object e)
        {
            if (this.TotalTicks > 0 && ++this._tickCount >= this.TotalTicks)
            {
                this.StopTimer();
            }

            // Raise the actions
            Interaction.ExecuteActions(AssociatedObject, this.Actions, null);
        }

        /// <summary>
        /// Called to start the timer
        /// </summary>
        internal void StartTimer()
        {
            this._timer = new DispatcherTimer { Interval = (TimeSpan.FromMilliseconds(this.MillisecondsPerTick)) };
            this._timer.Tick += this.OnTimerTick;
            this._timer.Start();
        }

        /// <summary>
        /// Called to stop the timer
        /// </summary>
        internal void StopTimer()
        {
            if (this._timer != null)
            {
                this._timer.Stop();
                this._timer = null;
            }
        }

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="associatedObject">The <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="T:Microsoft.Xaml.Interactivity.IBehavior"/> will be attached.</param>
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

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            this.StopTimer();
            AssociatedObject = null;
        }

        /// <summary>
        /// Gets the <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="T:Microsoft.Xaml.Interactivity.IBehavior"/> is attached.
        /// </summary>
        public DependencyObject AssociatedObject { get; private set; }
    }


    [ContentProperty(Name = "Actions")]
    [TypeConstraint(typeof(TextBox))]
    public class TextBoxEnterBehavior : DependencyObject, IBehavior
    {
        private TextBox AssociatedTextBox { get { return AssociatedObject as TextBox; } }
        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            AssociatedTextBox.KeyDown += AssociatedTextBox_KeyDown;
        }

        public void Detach()
        {
            AssociatedTextBox.KeyDown -= AssociatedTextBox_KeyDown;
        }

        private void AssociatedTextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Interaction.ExecuteActions(AssociatedObject, this.Actions, null);
            }
        }

        public ActionCollection Actions
        {
            get
            {
                var actions = (ActionCollection)base.GetValue(ActionsProperty);
                if (actions == null)
                {
                    base.SetValue(ActionsProperty, actions = new ActionCollection());
                }
                return actions;
            }
        }
        public static readonly DependencyProperty ActionsProperty = 
            DependencyProperty.Register("Actions", typeof(ActionCollection), 
                typeof(TextBoxEnterBehavior), new PropertyMetadata(null));
    }
}
