using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Microsoft.Xaml.Interactivity;
using Template10.Common;

namespace Template10.Behaviors
{
    /// <summary>
    /// Delayed actions trigger after a text change in associated text box. Actions is triggered only after a last change. If user type more text before delay time period exhause, delay is renewed.
    /// So, this behavior is very useful in a search text boxes. This behavior let user type text and search can be applied only after user finish typing. This behavior is very useful for an optimization purposes.
    /// </summary>
    [TypeConstraint(typeof(TextBox))]
    [ContentProperty(Name = nameof(Actions))]
    public sealed class TextBoxChangeDelayBehavior : DependencyObject, IBehavior
    {
        private TextBox AssociatedTextBox => AssociatedObject as TextBox;

        private IDispatcherWrapper dispatcherObj;

        private IDispatcherWrapper DispatcherObj
        {
            get
            {
                if (dispatcherObj == null)
                {
                    dispatcherObj = DispatcherWrapper.Current();
                }
                return dispatcherObj;
            }
        }

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="associatedObject">The <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which the <seealso cref="T:Microsoft.Xaml.Interactivity.IBehavior"/> will be attached.</param>
        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            if (AssociatedTextBox != null)
            {
                AssociatedTextBox.TextChanged += TextBoxOnTextChanged;
            }
        }

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            if (AssociatedTextBox != null)
            {
                AssociatedTextBox.TextChanged -= TextBoxOnTextChanged;
            }
            AssociatedObject = null;
        }

        private bool isWaiting;

        private bool isRefreshed;

        private DateTime stamp;

        private void TextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            DispatcherObj.Dispatch(async () =>
            {
                try
                {
                    if (sender != AssociatedObject)
                    {
                        return;
                    }
                    await DelayAction();
                }
                catch
                {
                    // Ignore exceptions: behavior SHOULD NOT crash program even if something wrong is going inside a behavior.
                    // UI behavior is not significant to a program logic and even if behavior logic broken due to something wrong, we can tolerate it.
                }
            });
        }

        private async Task DelayAction()
        {
            stamp = DateTime.Now;
            isRefreshed = true;
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
                    var toWait = (stamp.AddSeconds(DelaySeconds) - DateTime.Now);
                    if (toWait.Ticks > 0)
                    {
                        await Task.Delay(toWait);
                    }
                }
            }
            finally
            {
                isWaiting = false;
            }
            if (AssociatedObject != null)
            {
                Interaction.ExecuteActions(AssociatedObject, this.Actions, null);
            }
        }

        /// <summary>
        /// Associated object.
        /// </summary>
        public DependencyObject AssociatedObject
        {
            get { return (DependencyObject)GetValue(AssociatedObjectProperty); }
            set { SetValue(AssociatedObjectProperty, value); }
        }

        /// <summary>
        /// Associated object.
        /// </summary>
        public static readonly DependencyProperty AssociatedObjectProperty = DependencyProperty.Register("AssociatedObject", typeof(DependencyObject), typeof(TextBoxChangeDelayBehavior),
            new PropertyMetadata(null));

        /// <summary>
        /// Delay in seconds before actions is triggered.
        /// </summary>
        public double DelaySeconds
        {
            get
            {
                return (double)GetValue(DelaySecondsProperty);
            }
            set { SetValue(DelaySecondsProperty, value); }
        }

        /// <summary>
        /// Delay in seconds before actions is triggered.
        /// </summary>
        public static readonly DependencyProperty DelaySecondsProperty = DependencyProperty.Register("DelaySeconds", typeof(double), typeof(TextBoxChangeDelayBehavior),
            new PropertyMetadata(1.0));


        /// <summary>
        /// Actions.
        /// </summary>
        public ActionCollection Actions
        {
            get
            {
                var actions = (ActionCollection)GetValue(ActionsProperty);
                if (actions == null)
                {
                    base.SetValue(ActionsProperty, actions = new ActionCollection());
                }
                return actions;
            }
        }

        /// <summary>
        /// Actions.
        /// </summary>
        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(ActionCollection), typeof(TextBoxChangeDelayBehavior),
            new PropertyMetadata(null));

    }
}
