using System;
using System.Linq;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace Template10.App
{

    public enum ActivationKinds
    {
        Unknown,
        Resuming,
        Prelaunch,
        PrimaryTile,
        SecondaryTile,
        JumpList,
        ShareTarget,
        Search,
        RestrictedLaunch,
        ProtocolForResults,
        Protocol,
        PickerReturned,
        LockScreen,
        Launch,
        FileSavePicker,
        FileOpenPicker,
        File,
        DialReceiver,
        DevicePairing,
        Device,
        CachedFileUpdater,
        AppointmentsProviderShowTimeFrame,
        AppointmentsProviderShowAppointmentDetails,
        AppointmentsProviderReplaceAppointment,
        AppointmentsProviderRemoveAppointment,
        AppointmentsProviderAddAppointment,
        ToastNotification,
        VoiceCommand,
        WebAccountProvider,
    }

}