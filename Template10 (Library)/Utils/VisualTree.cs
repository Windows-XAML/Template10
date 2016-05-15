using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using System.Linq;

namespace Template10.Utils
{
    public static class VisualTree
    {

        /// <summary>
        /// Gets the first ancestor that is of the given type.
        /// </summary>
        /// <remarks>
        /// Returns null if not found.
        /// </remarks>
        /// <typeparam name="T">Type of ancestor to look for.</typeparam>
        /// <param name="d">The DependencyObject where the Visual Tree starts.</param>
        /// <returns></returns>
        public static T GetFirstAncestorOfType<T>(this DependencyObject d) where T : DependencyObject
        {
            return d.GetAncestorsOfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the the ancestors of a given type.
        /// </summary>
        /// <typeparam name="T">Type of ancestor to look for.</typeparam>
        /// <param name="d">The DependencyObject where the Visual Tree starts.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetAncestorsOfType<T>(this DependencyObject d) where T : DependencyObject
        {
            return d.GetAncestors().OfType<T>();
        }

        /// <summary>
        /// Gets the ancestors.
        /// </summary>
        /// <param name="d">The DependencyObject where the Visual Tree starts.</param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject start)
        {
            var parent = VisualTreeHelper.GetParent(start);

            while (parent != null)
            {
                yield return parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }
    }
}
