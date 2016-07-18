using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    [ContentProperty(Name = nameof(Actions))]
    [TypeConstraint(typeof(FrameworkElement))]
    public class VisbilityChangeBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; private set; }

        // The value of visibility that triggers the behavior
        public Visibility Visibility { get; set; } = Visibility.Visible;

        private Visibility? LastKnownVisibility { get; set; }

        private bool HasBeenLoaded { get; set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            (AssociatedObject as FrameworkElement).LayoutUpdated += VisbilityBehavior_LayoutUpdated;
            (AssociatedObject as FrameworkElement).Loaded += VisbilityBehavior_Loaded;
        }
        public void Detach()
        {
            (AssociatedObject as FrameworkElement).LayoutUpdated -= VisbilityBehavior_LayoutUpdated;
            (AssociatedObject as FrameworkElement).Loaded -= VisbilityBehavior_Loaded;
        }

        private void VisbilityBehavior_Loaded(object sender, RoutedEventArgs e)
        {
            HasBeenLoaded = true;
        }


        private void VisbilityBehavior_LayoutUpdated(object sender, object e)
        {
            if (!HasBeenLoaded)
                return;

            var element = (AssociatedObject as FrameworkElement);

            if (element?.Parent == null)
            {
                //If no parent navigation probably happening to a different page
                //Clear LastKnownVisibility to make sure actions are triggered
                //if we navigate back to page.
                LastKnownVisibility = null;
                HasBeenLoaded = false;
                return;
            }

            Visibility currentVisibility = Visibility.Visible;
            while (element != null)
            {
                if (element.Visibility == Visibility.Collapsed)
                {
                    currentVisibility = Visibility.Collapsed;
                    break;
                }

                element = element.Parent as FrameworkElement;
            }

            if (currentVisibility != LastKnownVisibility)
            {
                LastKnownVisibility = currentVisibility;

                if (LastKnownVisibility == Visibility)
                {
                    Interaction.ExecuteActions(AssociatedObject, Actions, null);
                }
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
