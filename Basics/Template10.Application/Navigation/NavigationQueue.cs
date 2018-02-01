using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Navigation
{
    public class NavigationQueue : Queue<NavigationInfo>
    {
        private static PageProvider _pageProvider;
        private static ViewModelProvider _viewModelProvider;

        public NavigationQueue(IEnumerable<NavigationInfo> collection)
            : base(collection.OrderBy(x => x.Index))
        {
            // empty
        }

        public bool ClearBackStack { get; set; }

        public override string ToString()
        {
            var prefix = ClearBackStack ? "/" : string.Empty;
            return $"{prefix}{string.Join("/", ToArray().Select(x => x.ToString()))}";
        }

        static NavigationQueue()
        {
            _pageProvider = new PageProvider();
            _viewModelProvider = new ViewModelProvider();
        }

        public static NavigationQueue Parse(string path, INavigationParameters parameters)
           => TryParse(path, parameters, out var queue) ? queue : throw new Exception();

        public static NavigationQueue Parse(Uri path, INavigationParameters parameters)
            => TryParse(path, parameters, out var queue) ? queue : throw new Exception();

        public static bool TryParse(string path, INavigationParameters parameters, out NavigationQueue queue)
        {
            if (string.IsNullOrEmpty(path))
            {
                queue = null;
                return false;
            }

            if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var uri))
            {
                return TryParse(new Uri(path, UriKind.Relative), parameters, out queue);
            }
            else
            {
                queue = null;
                return false;
            }
        }

        public static bool TryParse(Uri path, INavigationParameters parameters, out NavigationQueue queue)
        {
            if (path == null)
            {
                queue = null;
                return false;
            }

            if (path.IsAbsoluteUri)
            {
                throw new Exception("Navigation path must not be absolute Uri.");
            }

            var groups = path.OriginalString
                .Split("/")
                .Where(x => !string.IsNullOrEmpty(x))
                .Select((x, index) => new NavigationInfo(x, parameters)
                {
                    Index = index,
                    PageType = _pageProvider.Provider?.Invoke(x.Split('?').First()),
                    ViewModelType = _viewModelProvider.Provider?.Invoke(x.Split('?').First()),
                });

            queue = new NavigationQueue(groups)
            {
                ClearBackStack = path.OriginalString.StartsWith("/"),
            };

            return queue.Any();
        }
    }
}
