using Microsoft.Xaml.Interactivity;
using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    [ContentProperty(Name = nameof(Actions))]
    [TypeConstraint(typeof(UIElement))]
    public class KeyBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; private set; }

        // The key that triggers the behavior
        public VirtualKey Key { get; set; } = VirtualKey.None;
        // true if Control must be held also; otherwise false
        public bool AndControl { get; set; } = false;
        // true if Alt must be held also; otherwise false
        public bool AndAlt { get; set; } = false;
        // true if Shift must be held also; otherwise false
        public bool AndShift { get; set; } = false;
        // Detemines if the behavior is triggered on the KeyUp or the KeyDown event
        public Kinds Event { get; set; } = Kinds.KeyUp;

        public enum Kinds { KeyUp, KeyDown }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            if (Event == Kinds.KeyUp)
                (AssociatedObject as UIElement).KeyUp += UIElement_KeyHandler;
            else
                (AssociatedObject as UIElement).KeyDown += UIElement_KeyHandler;
        }

        public void Detach()
        {
            (AssociatedObject as UIElement).KeyUp -= UIElement_KeyHandler;
            (AssociatedObject as UIElement).KeyDown -= UIElement_KeyHandler;
        }

        private void UIElement_KeyHandler(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (AndControl)
            {
                var controlKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
                var ctrl = (controlKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                if (!ctrl) return;
            }

            if (AndShift)
            {
                var shiftKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift);
                var shift = (shiftKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                if (!shift) return;
            }

            if (AndAlt)
            {
                var altKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu);
                var alt = (altKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                if (!alt) return;
            }

            if (e.Key == Key)
            {
                Interaction.ExecuteActions(AssociatedObject, Actions, null);
                e.Handled = true;
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
                typeof(KeyBehavior), new PropertyMetadata(null));
    }
}
