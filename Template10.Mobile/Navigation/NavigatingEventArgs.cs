using System;
using Template10.Portable.Common;

namespace Template10.Portable.Navigation
{
    public class NavigatingEventArgs : NavigatedEventArgs
    {
        DeferralManager Manager;
        public Deferral GetDeferral() => Manager.GetDeferral();

        public NavigatingEventArgs(DeferralManager manager) : base()
        {
            Manager = manager;
        }

        public bool Cancel { get; set; } = false;
        public bool Suspending { get; set; } = false;
        public Type TargetPageType { get; set; }
        public NavigationParameters TargetPageParameter { get; set; }
    }
}