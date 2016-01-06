using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Behaviors
{
    public class FocusAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var ui = sender as Control;
            if (ui != null)
                ui.Focus(FocusState.Programmatic);
            return null;
        }
    }
}
