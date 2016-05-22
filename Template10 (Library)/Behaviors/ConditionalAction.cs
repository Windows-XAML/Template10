using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-XamlBehaviors
    [ContentProperty(Name = nameof(Actions))]
    public sealed class ConditionalAction : DependencyObject, IAction
    {
        public enum Operators
        {
            EqualToRight = 0,
            NotEqualToRight = 1,
            LessThanRight = 2,
            LessThanOrEqualToRight = 3,
            GreaterThanRight = 4,
            GreaterThanOrEqualToRight = 5,
            IsNull = 6,
            IsNotNull = 7,
            IsTrue = 8,
            IsFalse = 9,
            IsNullOrEmpty = 10,
            IsNotNullOrEmpty = 11
        }

        public Operators Operator
        {
            get { return (Operators)GetValue(OperatorProperty); }
            set { SetValue(OperatorProperty, value); }
        }
        public static readonly DependencyProperty OperatorProperty =
            DependencyProperty.Register(nameof(Operator), typeof(Operators),
                typeof(ConditionalAction), new PropertyMetadata(Operators.EqualToRight));

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
                    SetValue(ActionsProperty, actions);
                }
                return actions;
            }
        }
        public static readonly DependencyProperty ActionsProperty =
            DependencyProperty.Register(nameof(Actions), typeof(ActionCollection),
                typeof(TimeoutAction), new PropertyMetadata(null));

        int Compare<T>(T left, T right) => System.Collections.Generic.Comparer<T>.Default.Compare(left, right);

        public object Execute(object sender, object parameter)
        {
            var leftType = LeftValue?.GetType();
            var rightValue = (RightValue == null) ? null : Convert.ChangeType(RightValue, leftType);
            switch (Operator)
            {
                case Operators.EqualToRight:
                default:
                    if (Compare(LeftValue, rightValue) == 0)
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
                case Operators.NotEqualToRight:
                    if (Compare(LeftValue, rightValue) != 0)
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
                case Operators.LessThanRight:
                    if (Compare(LeftValue, rightValue) > 0)
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
                case Operators.LessThanOrEqualToRight:
                    if (Compare(LeftValue, rightValue) >= 0)
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
                case Operators.GreaterThanRight:
                    if (Compare(LeftValue, rightValue) < 0)
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
                case Operators.GreaterThanOrEqualToRight:
                    if (Compare(LeftValue, rightValue) <= 0)
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
                case Operators.IsNull:
                    if (LeftValue == null)
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
                case Operators.IsNotNull:
                    if (LeftValue != null)
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
                case Operators.IsTrue:
                    if ((bool?)LeftValue ?? false)
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
                case Operators.IsFalse:
                    if (!(bool?)LeftValue ?? false)
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
                case Operators.IsNullOrEmpty:
                    if (string.IsNullOrEmpty(LeftValue as string))
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
                case Operators.IsNotNullOrEmpty:
                    if (!string.IsNullOrEmpty(LeftValue as string))
                        Interaction.ExecuteActions(sender, Actions, parameter);
                    break;
            }
            return null;
        }
    }
}
