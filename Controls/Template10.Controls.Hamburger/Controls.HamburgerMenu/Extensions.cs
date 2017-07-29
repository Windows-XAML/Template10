using Template10.Common;
using Windows.UI.Xaml;

namespace Template10.Controls
{
    internal static class Extensions
    {
        public static ChangedEventArgs<T> ToChangedEventArgs<T>(this DependencyPropertyChangedEventArgs e)
            => new ChangedEventArgs<T>((T)e.OldValue, (T)e.NewValue);
    }
}
