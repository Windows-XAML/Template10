using System;

namespace Template10.Services.Navigation
{

    public interface INavigatingArgs
    {
        NavigationModes NavigationMode { get; set; }
        object NavigationTransitionInfo { get; set; }
        object Parameter { get; set; }
        Type SourcePageType { get; set; }
    }

}