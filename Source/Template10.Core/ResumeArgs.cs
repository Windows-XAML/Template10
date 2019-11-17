using System;
using Windows.ApplicationModel.Activation;

namespace Template10
{

    public class ResumeArgs : IResumeArgs, IActivatedEventArgs
    {
        public ActivationKind Kind { get; set; }
        public ApplicationExecutionState PreviousExecutionState { get; set; }
        public SplashScreen SplashScreen { get; set; }
        public DateTime? SuspendDate { get; set; }
    }
}
