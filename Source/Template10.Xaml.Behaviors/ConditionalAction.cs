using System.Linq;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Behaviors-and-Actions
    [ContentProperty(Name = nameof(Actions))]
    public sealed class ConditionalAction : DependencyObject, IAction
    {
        public ConditionCollection Conditions
        {
            get
            {
                if (!(base.GetValue(ConditionsProperty) is ConditionCollection actions))
                {
                    actions = new ConditionCollection();
                    SetValue(ConditionsProperty, actions);
                }
                return actions;
            }
        }
        public static readonly DependencyProperty ConditionsProperty =
            DependencyProperty.Register(nameof(Conditions), typeof(ConditionCollection),
                typeof(ConditionalAction), new PropertyMetadata(null));

        public enum Operators
        {
            AllTrue = 0,
            AnyTrue = 1,
            NoneTrue = 2
        }

        public Operators Operator
        {
            get => (Operators)GetValue(OperatorProperty);
            set => SetValue(OperatorProperty, value);
        }
        public static readonly DependencyProperty OperatorProperty =
            DependencyProperty.Register(nameof(Operator), typeof(Operators),
                typeof(Condition), new PropertyMetadata(Operators.AllTrue));

        public ActionCollection Actions
        {
            get
            {
                if (!(base.GetValue(ActionsProperty) is ActionCollection actions))
                {
                    actions = new ActionCollection();
                    SetValue(ActionsProperty, actions);
                }
                return actions;
            }
        }
        public static readonly DependencyProperty ActionsProperty =
            DependencyProperty.Register(nameof(Actions), typeof(ActionCollection),
                typeof(ConditionalAction), new PropertyMetadata(null));

        public ActionCollection Else
        {
            get
            {
                if (!(base.GetValue(ElseProperty) is ActionCollection actions))
                {
                    actions = new ActionCollection();
                    SetValue(ElseProperty, actions);
                }
                return actions;
            }
        }
        public static readonly DependencyProperty ElseProperty =
            DependencyProperty.Register(nameof(Else), typeof(ActionCollection),
                typeof(ConditionalAction), new PropertyMetadata(null));

        public object Execute(object sender, object parameter)
        {
            var true_results = Conditions.Where(x => x.Execute()).Count();
            switch (Operator)
            {
                case Operators.AllTrue when Equals(Conditions.Count(), true_results):
                    return Interaction.ExecuteActions(sender, Actions, parameter);
                case Operators.AnyTrue when !Equals(0, true_results):
                    return Interaction.ExecuteActions(sender, Actions, parameter);
                case Operators.NoneTrue when Equals(0, true_results):
                    return Interaction.ExecuteActions(sender, Actions, parameter);
                default:
                    return Interaction.ExecuteActions(sender, Else, parameter);
            }
        }
    }
}
