using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Behaviors
{
    public class FocusAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            Control ui;
            if (TargetObject != null)
                ui = TargetObject;
            else
                ui = sender as Control;
            if (ui != null)
                ui.Focus(FocusState.Programmatic);
            return null;
        }

        public Control TargetObject { get; set; }
    }
}
