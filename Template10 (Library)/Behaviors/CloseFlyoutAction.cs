using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Template10.Behaviors
{
    public class CloseFlyoutAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var parent = TargetObject ?? sender as DependencyObject;
            while (parent != null)
            {
                if (parent is FlyoutPresenter)
                {
                    ((parent as FlyoutPresenter).Parent as Popup).IsOpen = false;
                    break;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }
            return null;
        }

        public Control TargetObject { get; set; }
    }
}
