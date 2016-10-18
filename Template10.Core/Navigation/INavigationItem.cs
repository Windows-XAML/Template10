using System;

namespace Template10.Navigation
{

    public interface INavigationItem
    {
        Type Page { get; set; }
        object Parameter { get; set; }
    }

}