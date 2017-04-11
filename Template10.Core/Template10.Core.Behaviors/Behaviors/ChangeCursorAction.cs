using Microsoft.Xaml.Interactivity;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Template10.Behaviors
{
    public class CursorBehavior : DependencyObject, IAction
    {
        public CoreCursorType Cursor { get; set; } = CoreCursorType.Arrow;

        public object Execute(object sender, object parameter)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(Cursor, 0);
            return null;
        }
    }
}
