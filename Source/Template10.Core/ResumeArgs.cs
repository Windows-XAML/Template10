using System;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace Template10
{

    public class ResumeArgs : IResumeArgs, IActivatedEventArgs
    {
        public ActivationKind Kind { get; set; }
        public ApplicationExecutionState PreviousExecutionState { get; set; }
        public SplashScreen SplashScreen { get; set; }
        public ResumeArgs SetSuspensionDate(DateTime? date)
        {
            SuspensionDate = date;
            return this;
        }
        public DateTime? SuspensionDate { get; set; }
        internal static ResumeArgs Create(ApplicationExecutionState state)
        {
            var args = new ResumeArgs
            {
                PreviousExecutionState = state
            };
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue("Suspend_Data", out var value) && value is DateTime date)
            {
                args.SuspensionDate = date;
            }
            ApplicationData.Current.LocalSettings.Values.Remove("Suspend_Data");
            return args;
        }
    }
}
