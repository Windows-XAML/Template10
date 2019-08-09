using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace Template10
{
    public static class SuspensionUtilities
    {
        internal static bool IsResuming(StartArgs startArgs, out ResumeArgs resumeArgs)
        {
            if (WasTerminated(startArgs) && WasSuspended())
            {
                resumeArgs = ResumeArgs.Create(ApplicationExecutionState.Terminated);
                resumeArgs.SuspendDate = GetSuspendDate();
                ClearSuspendDate();
                return true;
            }
            resumeArgs = null;
            return false;
        }

        internal static bool WasTerminated(StartArgs startArgs)
        {
            return startArgs.Arguments is ILaunchActivatedEventArgs e
                && e.PreviousExecutionState == ApplicationExecutionState.Terminated;
        }

        internal static bool WasSuspended()
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey("Suspend_Data");
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
