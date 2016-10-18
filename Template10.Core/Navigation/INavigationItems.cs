using System.Collections.Generic;

namespace Template10.Navigation
{

    public interface INavigationItems: IEnumerable<INavigationItem>
    {
        void Clear();
    }

}