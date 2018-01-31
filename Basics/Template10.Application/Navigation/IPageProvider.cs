using System;

namespace Template10.Navigation
{
    public interface IPageProvider
    {
        Func<string, Type> Provider { get; set; }
    }
}
