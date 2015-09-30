using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-XamlBehaviors
    [ContentProperty(Name = nameof(Actions))]
    public sealed class ConditionalAction : DependencyObject, IAction
    {
        public ComparisonConditionType Operator
        {
            get { return (ComparisonConditionType)GetValue(OperatorProperty); }
            set { SetValue(OperatorProperty, value); }
        }
        public static readonly DependencyProperty OperatorProperty =
            DependencyProperty.Register(nameof(Operator), typeof(ComparisonConditionType),
                typeof(ConditionalAction), new PropertyMetadata(ComparisonConditionType.Equal));

        public object LeftValue
        {
            get { return (object)GetValue(LeftValueProperty); }
            set { SetValue(LeftValueProperty, value); }
        }
        public static readonly DependencyProperty LeftValueProperty =
            DependencyProperty.Register(nameof(LeftValue), typeof(object),
                typeof(ConditionalAction), new PropertyMetadata(null));

        public object RightValue
        {
            get { return (object)GetValue(RightValueProperty); }
            set { SetValue(RightValueProperty, value); }
        }
        public static readonly DependencyProperty RightValueProperty =
            DependencyProperty.Register(nameof(RightValue), typeof(object),
                typeof(ConditionalAction), new PropertyMetadata(null));

        public ActionCollection Actions
        {
            get
            {
                var actions = base.GetValue(ActionsProperty) as ActionCollection;
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
                typeof(TimeoutAction), new PropertyMetadata(null));

        int Compare<T>(T left, T right)
        {
            return System.Collections.Generic.Comparer<T>.Default.Compare(left, right);
        }

        public object Execute(object sender, object parameter)
        {
            if (LeftValue == null && RightValue == null)
            {
                Interaction.ExecuteActions(this, this.Actions, null);
            }
            else if (LeftValue == null && RightValue != null)
            {
                // nothing
            }
            else if (LeftValue != null && RightValue == null)
            {
                // nothing
            }
            else
            {
                var leftType = LeftValue.GetType();
                var rightValue = Convert.ChangeType(RightValue, leftType);
                switch (Operator)
                {
                    case ComparisonConditionType.Equal:
                    default:
                        if (Compare(LeftValue, rightValue) == 0)
                            Interaction.ExecuteActions(this, this.Actions, null);
                        break;
                    case ComparisonConditionType.NotEqual:
                        if (Compare(LeftValue, rightValue) != 0)
                            Interaction.ExecuteActions(this, this.Actions, null);
                        break;
                    case ComparisonConditionType.LessThan:
                        if (Compare(LeftValue, rightValue) > 0)
                            Interaction.ExecuteActions(this, this.Actions, null);
                        break;
                    case ComparisonConditionType.LessThanOrEqual:
                        if (Compare(LeftValue, rightValue) >= 0)
                            Interaction.ExecuteActions(this, this.Actions, null);
                        break;
                    case ComparisonConditionType.GreaterThan:
                        if (Compare(LeftValue, rightValue) < 0)
                            Interaction.ExecuteActions(this, this.Actions, null);
                        break;
                    case ComparisonConditionType.GreaterThanOrEqual:
                        if (Compare(LeftValue, rightValue) <= 0)
                            Interaction.ExecuteActions(this, this.Actions, null);
                        break;
                }
            }
            return null;
        }
    }
}
