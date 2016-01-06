using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Template10.Common;

namespace Template10.Behaviors
{
    [ContentProperty(Name = nameof(Actions))]
    public class BackButtonBehavior : DependencyObject, IBehavior
    {
        private DispatcherWrapper _dispatcher;
        public DependencyObject AssociatedObject { get; set; }

        public void Attach(DependencyObject associatedObject)
        {
            _dispatcher = WindowWrapper.Current().Dispatcher;
            BootStrapper.BackRequested += BootStrapper_BackRequested;
        }

        public void Detach()
        {
            BootStrapper.BackRequested -= BootStrapper_BackRequested;
        }

        private void BootStrapper_BackRequested(object sender, HandledEventArgs e)
        {
            e.Handled = Handled;
            foreach (IAction item in Actions)
            {
                _dispatcher.Dispatch(() => item.Execute(sender, null));
            }
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
                    base.SetValue(ActionsProperty, actions = new ActionCollection());
                }
                return actions;
            }
        }
        public static readonly DependencyProperty ActionsProperty =
            DependencyProperty.Register(nameof(Actions), typeof(ActionCollection),
                typeof(TextBoxEnterKeyBehavior), new PropertyMetadata(null));
    }
}
