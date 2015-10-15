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
        public enum Operators
        {
            Equal = 0,
            NotEqual = 1,
            LessThan = 2,
            LessThanOrEqual = 3,
            GreaterThan = 4,
            GreaterThanOrEqual = 5,
            IsNull = 6,
            IsNotNull = 7,
        }

        public Operators Operator
        {
            get { return (Operators)GetValue(OperatorProperty); }
            set { SetValue(OperatorProperty, value); }
        }
        public static readonly DependencyProperty OperatorProperty =
            DependencyProperty.Register(nameof(Operator), typeof(Operators),
                typeof(ConditionalAction), new PropertyMetadata(Operators.Equal));

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

        int Compare<T>(T left, T right) => System.Collections.Generic.Comparer<T>.Default.Compare(left, right);

        public object Execute(object sender, object parameter)
        {
            var leftType = LeftValue?.GetType();
            var rightValue = (RightValue == null) ? null : Convert.ChangeType(RightValue, leftType);
            switch (Operator)
            {
                case Operators.Equal:
                default:
                    if (Compare(LeftValue, rightValue) == 0)
                        Interaction.ExecuteActions(this, this.Actions, null);
                    break;
                case Operators.NotEqual:
                    if (Compare(LeftValue, rightValue) != 0)
                        Interaction.ExecuteActions(this, this.Actions, null);
                    break;
                case Operators.LessThan:
                    if (Compare(LeftValue, rightValue) > 0)
                        Interaction.ExecuteActions(this, this.Actions, null);
                    break;
                case Operators.LessThanOrEqual:
                    if (Compare(LeftValue, rightValue) >= 0)
                        Interaction.ExecuteActions(this, this.Actions, null);
                    break;
                case Operators.GreaterThan:
                    if (Compare(LeftValue, rightValue) < 0)
                        Interaction.ExecuteActions(this, this.Actions, null);
                    break;
                case Operators.GreaterThanOrEqual:
                    if (Compare(LeftValue, rightValue) <= 0)
                        Interaction.ExecuteActions(this, this.Actions, null);
                    break;
                case Operators.IsNull:
                    if (LeftValue == null)
                        Interaction.ExecuteActions(this, this.Actions, null);
                    break;
                case Operators.IsNotNull:
                    if (LeftValue != null)
                        Interaction.ExecuteActions(this, this.Actions, null);
                    break;
            }
            return null;
        }
    }
}
