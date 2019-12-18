using System.Threading.Tasks;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Behaviors
{
    [ContentProperty(Name = nameof(Actions))]
    public sealed class ThrottledAction : DependencyObject, IAction
    {
        /// <summary>
        /// Minimum Period in Milliseconds
        /// </summary>
        public int Milliseconds
        {
            get { return (int)GetValue(MillisecondsProperty); }
            set { SetValue(MillisecondsProperty, value); }
        }
        public static readonly DependencyProperty MillisecondsProperty =
            DependencyProperty.Register(nameof(Milliseconds),
                typeof(int), typeof(ThrottledAction), new PropertyMetadata(0));

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
                typeof(ThrottledAction), new PropertyMetadata(null));

        private bool Busy { get; set; }

        public object Execute(object sender, object parameter)
        {
            if (Busy)
            {
                return null;
            }
            Busy = true;
            Interaction.ExecuteActions(sender, Actions, parameter);
            return Task.Run(async () =>
            {
                await Task.Delay(Milliseconds);
                Busy = false;
            });
        }
    }
}
