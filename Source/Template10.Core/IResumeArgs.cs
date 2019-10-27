using System;
using Windows.ApplicationModel.Activation;

namespace Template10
{
    public interface IResumeArgs
    {
        ApplicationExecutionState PreviousExecutionState { get; set; }
        ActivationKind Kind { get; set; }
        DateTime? SuspendDate { get; set; }
    }
}
