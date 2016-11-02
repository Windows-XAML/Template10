using System;

namespace Template10.Services.Navigation
{

    public interface INavigatedArgs
    {
        object Content { get; set; }
        NavigationModes NavigationMode { get; set; }
        object NavigationTransitionInfo { get; set; }
        object Parameter { get; set; }
        Type SourcePageType { get; set; }
        Uri Uri { get; set; }
    }

}