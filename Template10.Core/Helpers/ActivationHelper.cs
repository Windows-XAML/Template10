using Windows.ApplicationModel.Activation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Helpers
{
    public static class ActivationHelper
    {
        public static ShareTargetActivatedEventArgs ToShareTargetActivatedEventArgs(this IActivatedEventArgs args) => args as ShareTargetActivatedEventArgs;
        public static SearchActivatedEventArgs ToSearchActivatedEventArgs(this IActivatedEventArgs args) => args as SearchActivatedEventArgs;
        public static RestrictedLaunchActivatedEventArgs ToRestrictedLaunchActivatedEventArgs(this IActivatedEventArgs args) => args as RestrictedLaunchActivatedEventArgs;
        public static ProtocolForResultsActivatedEventArgs ToProtocolForResultsActivatedEventArgs(this IActivatedEventArgs args) => args as ProtocolForResultsActivatedEventArgs;
        public static ProtocolActivatedEventArgs ToProtocolActivatedEventArgs(this IActivatedEventArgs args) => args as ProtocolActivatedEventArgs;
        public static PickerReturnedActivatedEventArgs ToPickerReturnedActivatedEventArgs(this IActivatedEventArgs args) => args as PickerReturnedActivatedEventArgs;
        public static LockScreenActivatedEventArgs ToLockScreenActivatedEventArgs(this IActivatedEventArgs args) => args as LockScreenActivatedEventArgs;
        public static LaunchActivatedEventArgs ToLaunchActivatedEventArgs(this IActivatedEventArgs args) => args as LaunchActivatedEventArgs;
        public static FileSavePickerActivatedEventArgs ToFileSavePickerActivatedEventArgs(this IActivatedEventArgs args) => args as FileSavePickerActivatedEventArgs;
        public static FileOpenPickerActivatedEventArgs ToFileOpenPickerActivatedEventArgs(this IActivatedEventArgs args) => args as FileOpenPickerActivatedEventArgs;
        public static FileActivatedEventArgs ToFileActivatedEventArgs(this IActivatedEventArgs args) => args as FileActivatedEventArgs;
        public static DialReceiverActivatedEventArgs ToDialReceiverActivatedEventArgs(this IActivatedEventArgs args) => args as DialReceiverActivatedEventArgs;
        public static DevicePairingActivatedEventArgs ToDevicePairingActivatedEventArgs(this IActivatedEventArgs args) => args as DevicePairingActivatedEventArgs;
        public static DeviceActivatedEventArgs ToDeviceActivatedEventArgs(this IActivatedEventArgs args) => args as DeviceActivatedEventArgs;
        public static CachedFileUpdaterActivatedEventArgs ToCachedFileUpdaterActivatedEventArgs(this IActivatedEventArgs args) => args as CachedFileUpdaterActivatedEventArgs;
        public static AppointmentsProviderShowTimeFrameActivatedEventArgs ToAppointmentsProviderShowTimeFrameActivatedEventArgs(this IActivatedEventArgs args) => args as AppointmentsProviderShowTimeFrameActivatedEventArgs;
        public static AppointmentsProviderShowAppointmentDetailsActivatedEventArgs ToAppointmentsProviderShowAppointmentDetailsActivatedEventArgs(this IActivatedEventArgs args) => args as AppointmentsProviderShowAppointmentDetailsActivatedEventArgs;
        public static AppointmentsProviderReplaceAppointmentActivatedEventArgs ToAppointmentsProviderReplaceAppointmentActivatedEventArgs(this IActivatedEventArgs args) => args as AppointmentsProviderReplaceAppointmentActivatedEventArgs;
        public static AppointmentsProviderRemoveAppointmentActivatedEventArgs ToAppointmentsProviderRemoveAppointmentActivatedEventArgs(this IActivatedEventArgs args) => args as AppointmentsProviderRemoveAppointmentActivatedEventArgs;
        public static AppointmentsProviderAddAppointmentActivatedEventArgs ToAppointmentsProviderAddAppointmentActivatedEventArgs(this IActivatedEventArgs args) => args as AppointmentsProviderAddAppointmentActivatedEventArgs;
        public static ToastNotificationActivatedEventArgs ToToastNotificationActivatedEventArgs(this IActivatedEventArgs args) => args as ToastNotificationActivatedEventArgs;
        public static VoiceCommandActivatedEventArgs ToVoiceCommandActivatedEventArgs(this IActivatedEventArgs args) => args as VoiceCommandActivatedEventArgs;
        public static WebAccountProviderActivatedEventArgs ToWebAccountProviderActivatedEventArgs(this IActivatedEventArgs args) => args as WebAccountProviderActivatedEventArgs;

        public static ActivationKinds DetermineStartupKind(IActivatedEventArgs args)
        {
            string DefaultTileID = "App";
            var e = args.ToLaunchActivatedEventArgs();
            if (e?.PrelaunchActivated == true)
            {
                return ActivationKinds.Prelaunch;
            }
            else if (args.PreviousExecutionState == ApplicationExecutionState.Suspended
                 || args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                return ActivationKinds.Resuming;
            }
            else if (e?.TileId == DefaultTileID && string.IsNullOrEmpty(e?.Arguments))
            {
                return ActivationKinds.PrimaryTile;
            }
            else if (e?.TileId == DefaultTileID && !string.IsNullOrEmpty(e?.Arguments))
            {
                return ActivationKinds.JumpList;
            }
            else if (!string.IsNullOrEmpty(e?.TileId) && e?.TileId != DefaultTileID)
            {
                return ActivationKinds.SecondaryTile;
            }
            else if (args.ToShareTargetActivatedEventArgs() != null) return ActivationKinds.ShareTarget;
            else if (args.ToSearchActivatedEventArgs() != null) return ActivationKinds.Search;
            else if (args.ToRestrictedLaunchActivatedEventArgs() != null) return ActivationKinds.RestrictedLaunch;
            else if (args.ToProtocolForResultsActivatedEventArgs() != null) return ActivationKinds.ProtocolForResults;
            else if (args.ToProtocolActivatedEventArgs() != null) return ActivationKinds.Protocol;
            else if (args.ToPickerReturnedActivatedEventArgs() != null) return ActivationKinds.PickerReturned;
            else if (args.ToLockScreenActivatedEventArgs() != null) return ActivationKinds.LockScreen;
            else if (args.ToFileSavePickerActivatedEventArgs() != null) return ActivationKinds.FileSavePicker;
            else if (args.ToFileOpenPickerActivatedEventArgs() != null) return ActivationKinds.FileOpenPicker;
            else if (args.ToFileActivatedEventArgs() != null) return ActivationKinds.File;
            else if (args.ToDialReceiverActivatedEventArgs() != null) return ActivationKinds.DialReceiver;
            else if (args.ToDevicePairingActivatedEventArgs() != null) return ActivationKinds.DevicePairing;
            else if (args.ToDeviceActivatedEventArgs() != null) return ActivationKinds.Device;
            else if (args.ToCachedFileUpdaterActivatedEventArgs() != null) return ActivationKinds.CachedFileUpdater;
            else if (args.ToAppointmentsProviderShowTimeFrameActivatedEventArgs() != null) return ActivationKinds.AppointmentsProviderShowTimeFrame;
            else if (args.ToAppointmentsProviderShowAppointmentDetailsActivatedEventArgs() != null) return ActivationKinds.AppointmentsProviderShowAppointmentDetails;
            else if (args.ToAppointmentsProviderReplaceAppointmentActivatedEventArgs() != null) return ActivationKinds.AppointmentsProviderReplaceAppointment;
            else if (args.ToAppointmentsProviderRemoveAppointmentActivatedEventArgs() != null) return ActivationKinds.AppointmentsProviderRemoveAppointment;
            else if (args.ToAppointmentsProviderAddAppointmentActivatedEventArgs() != null) return ActivationKinds.AppointmentsProviderAddAppointment;
            else if (args.ToToastNotificationActivatedEventArgs() != null) return ActivationKinds.ToastNotification;
            else if (args.ToVoiceCommandActivatedEventArgs() != null) return ActivationKinds.VoiceCommand;
            else if (args.ToWebAccountProviderActivatedEventArgs() != null) return ActivationKinds.WebAccountProvider;
            else return ActivationKinds.Unknown;

        }
    }
}
