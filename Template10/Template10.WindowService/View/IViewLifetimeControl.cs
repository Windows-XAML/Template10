using System;
using Template10.Services.WindowWrapper;
using Windows.UI.Core;

namespace Template10.Services.ViewService
{
    // A custom event that fires whenever the secondary view is ready to be closed. You should
    // clean up any state (including deregistering for events) then close the window in this handler
    public delegate void ViewReleasedHandler(object sender, EventArgs e);

    public interface IViewLifetimeControl
    {
        CoreDispatcher CoreDispatcher { get; }
        int Id { get; }
        // INavigationService NavigationService { get; set; }
        IWindowWrapper WindowWrapper { get; }

        event ViewReleasedHandler Released;

        int StartViewInUse();
        int StopViewInUse();
    }
}