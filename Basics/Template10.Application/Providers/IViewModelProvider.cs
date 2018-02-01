using System;

namespace Template10.Navigation
{
    public interface IViewModelProvider
    {
        Func<string, Type> Provider { get; set; }
    }
}
