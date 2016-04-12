using Microsoft.Xaml.Interactivity;
using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-XamlBehaviors
    [ContentProperty(Name = nameof(Actions))]
    [TypeConstraint(typeof(TextBox))]
    [Obsolete("Use KeyBehavior instead.")]
    public class TextBoxEnterKeyBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            (AssociatedObject as TextBox).KeyUp += AssociatedTextBox_KeyUp;
        }

        public void Detach()
        {
            (AssociatedObject as TextBox).KeyUp -= AssociatedTextBox_KeyUp;
        }

        private void AssociatedTextBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
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
                typeof(TextBoxEnterKeyBehavior), new PropertyMetadata(null));
    }
}
