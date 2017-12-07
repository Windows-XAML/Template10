using Microsoft.Xaml.Interactivity;
using Template10.Common;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    /// <summary>
    /// Use this behavior to add actions when the user clicks the back button
    /// Usually the shell back button. And, you are not necessarily wanting to 
    /// navigate, but do something like close a dialog box.
    /// </summary>

    [ContentProperty(Name = nameof(Actions))]
    public class BackButtonBehavior : DependencyObject, IBehavior
    {
        Services.Gesture.IGestureService _gestureService;
        public BackButtonBehavior()
        {
            _gestureService = Central.Gesture;
        }

        private IDispatcherEx _dispatcher;
        public DependencyObject AssociatedObject { get; set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            _dispatcher = WindowEx2.Current().Dispatcher;
            Central.Messenger.Subscribe<Messages.BackRequestedMessage>(this, e =>
            {
                Interaction.ExecuteActions(AssociatedObject, Actions, null);
            });
        }

        public void Detach()
        {
            // empty
        }

        public bool Handled
        {
            get { return (bool)GetValue(HandledProperty); }
            set { SetValue(HandledProperty, value); }
        }
        public static readonly DependencyProperty HandledProperty =
            DependencyProperty.Register(nameof(Handled), typeof(bool),
                typeof(BackButtonBehavior), new PropertyMetadata(false));

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
                typeof(BackButtonBehavior), new PropertyMetadata(null));
    }
}
