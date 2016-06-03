using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using System.Linq;

namespace Sample.Utils
{
    public static class VisualTree
    {
  
        public static T FirstDescendant<T>(this DependencyObject d) where T : DependencyObject
        {
            return d.Descendants().OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<DependencyObject> Descendants(this DependencyObject d)
        {
            var queue = new Queue<DependencyObject>();
            var count = VisualTreeHelper.GetChildrenCount(d);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(d, i);
                yield return child;
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                var parent = queue.Dequeue();
                var count2 = VisualTreeHelper.GetChildrenCount(parent);

                for (int i = 0; i < count2; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    yield return child;
                    queue.Enqueue(child);
                }
            }
        }
    }
}
