using System;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace Template10
{
    public static class SuspensionUtilities
    {
        internal static bool IsResuming(StartArgs startArgs, out ResumeArgs resumeArgs)
        {
            if (WasTerminated() && WasSuspended())
            {
                resumeArgs = new ResumeArgs
                {
                    PreviousExecutionState = ApplicationExecutionState.Terminated,
                    SuspendDate = GetSuspendDate(),
                };
                return true;
            }
            resumeArgs = null;
            return false;

            bool WasTerminated()
            {
                return startArgs.Arguments is ILaunchActivatedEventArgs e
                    && e.PreviousExecutionState == ApplicationExecutionState.Terminated;
            }

            bool WasSuspended()
            {
                return ApplicationData.Current.LocalSettings.Values.ContainsKey("Suspend_Data");
            }
        }

        internal static void ClearSuspendDate()
        {
            ApplicationData.Current.LocalSettings.Values.Remove("Suspend_Data");
        }

        internal static DateTime? GetSuspendDate()
        {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue("Suspend_Data", out var value)
                && value != null
                && DateTime.TryParse(value.ToString(), out var date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }

        internal static void SetSuspendDate(DateTime value)
        {
            ApplicationData.Current.LocalSettings.Values["Suspend_Data"] = value.ToString();
        }
    }
}
