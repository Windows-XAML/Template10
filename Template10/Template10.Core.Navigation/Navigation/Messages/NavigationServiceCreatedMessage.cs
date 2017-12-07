using Template10.Core;
using Template10.Navigation;

namespace Template10.Messages
{
    public class NavigationServiceCreatedMessage
    {
        public INavigationService NavigationService { get; set; }
        public bool IsDefault { get; set; }
        public BackButton BackButtonHandling { get; set; }
        public IDispatcherEx Dispatcher { get; set; }
    }
}
