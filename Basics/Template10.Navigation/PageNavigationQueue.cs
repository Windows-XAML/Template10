using System.Collections.Generic;
using System.Linq;

namespace Template10.Navigation
{
    public class PageNavigationQueue : Queue<PageNavigationInfo>
    {
        public PageNavigationQueue(IEnumerable<PageNavigationInfo> collection)
            : base(collection.OrderBy(x => x.Index))
        {
            // empty
        }

        public bool ClearBackStack { get; set; }
    }
}
