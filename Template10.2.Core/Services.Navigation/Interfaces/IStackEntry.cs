using System;

namespace Template10.Services.Navigation
{

    public interface IStackEntry
    {
        Type Page { get; set; }
        object Parameter { get; set; }
        Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo NavigationTransitionInfo { get; set; }
    }
}