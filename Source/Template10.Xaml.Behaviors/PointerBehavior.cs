using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    [TypeConstraint(typeof(UIElement))]
    [ContentProperty(Name = nameof(Actions))]
    public class PointerBehavior : DependencyObject, IBehavior
    {
        public enum EventKind { Enter, Exit, Move, Pressed, Released, Tapped, None }

        public DependencyObject AssociatedObject { get; private set; }
        private UIElement AssociatedElement => AssociatedObject as UIElement;

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            AssociatedElement.Tapped += AssociatedElement_Tapped;
            AssociatedElement.PointerEntered += AssociatedElement_PointerEntered;
            AssociatedElement.PointerExited += AssociatedElement_PointerExited;
            AssociatedElement.PointerMoved += AssociatedElement_PointerMoved;
            AssociatedElement.PointerPressed += AssociatedElement_PointerPressed;
            AssociatedElement.PointerReleased += AssociatedElement_PointerReleased;
        }

        #region Handlers

        private void AssociatedElement_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Event == EventKind.Tapped)
            {
                Interaction.ExecuteActions(AssociatedObject, Actions, null);
            }
        }

        private void AssociatedElement_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (Event == EventKind.Enter)
            {
                Interaction.ExecuteActions(AssociatedObject, Actions, null);
            }
        }

        private void AssociatedElement_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (Event == EventKind.Exit)
            {
                Interaction.ExecuteActions(AssociatedObject, Actions, null);
            }
        }

        private void AssociatedElement_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (Event == EventKind.Move)
            {
                Interaction.ExecuteActions(AssociatedObject, Actions, null);
            }
        }

        private void AssociatedElement_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (Event == EventKind.Pressed)
            {
                Interaction.ExecuteActions(AssociatedObject, Actions, null);
            }
        }

        private void AssociatedElement_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (Event == EventKind.Released)
            {
                Interaction.ExecuteActions(AssociatedObject, Actions, null);
            }
        }

        #endregion

        public void Detach()
        {
            AssociatedElement.PointerEntered -= AssociatedElement_PointerEntered;
            AssociatedElement.PointerExited -= AssociatedElement_PointerExited;
            AssociatedElement.PointerMoved -= AssociatedElement_PointerMoved;
            AssociatedElement.PointerPressed -= AssociatedElement_PointerPressed;
            AssociatedElement.PointerReleased -= AssociatedElement_PointerReleased;
        }

        public EventKind Event
        {
            get => (EventKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }
        public static readonly DependencyProperty KindProperty =
            DependencyProperty.Register(nameof(Event), typeof(EventKind),
                typeof(PointerBehavior), new PropertyMetadata(EventKind.None));

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
                typeof(PointerBehavior), new PropertyMetadata(null));
    }
}

