using System;
using Template10.Services.StateService;

namespace Template10.Portable.Navigation
{
    public interface INavigationInfo
    {
        Type PageType { get; }
        object Parameter { get; }
        NavigationMode Mode { get; }
        IStateContainer State { get; }
    }
}