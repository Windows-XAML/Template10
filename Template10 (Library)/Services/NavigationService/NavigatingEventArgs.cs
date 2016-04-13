using Template10.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public class NavigatingEventArgs : NavigatedEventArgs
    {
        DeferralManager Manager;
        public Deferral GetDeferral() => Manager.GetDeferral();

        public NavigatingEventArgs(DeferralManager manager) : base()
        {
            Manager = manager;
        }

        public NavigatingEventArgs(DeferralManager manager, NavigatingCancelEventArgs e, Page page, object parameter) : this(manager)
        {
            NavigationMode = e.NavigationMode;
            PageType = e.SourcePageType;
            Page = page;
            Parameter = parameter;
        }

        public bool Cancel { get; set; } = false;
        public bool Suspending { get; set; } = false;
    }
}