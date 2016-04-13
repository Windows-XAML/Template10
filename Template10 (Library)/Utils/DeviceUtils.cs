using System;
using System.Collections.Generic;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace Template10.Utils
{
    public class DeviceUtils
    {
        public event EventHandler Changed;

        public MonitorUtils MonitorUtils { get; private set; }

        public Common.WindowWrapper WindowWrapper { get; private set; }

        [Flags]
        public enum DeviceDispositions
        {
            Unknown = 0,
            IoT = 1 << 0,
            Xbox = 1 << 1,
            Team = 1 << 2,
            HoloLens = 1 << 3,
            Desktop = 1 << 4,
            Mobile = 1 << 5,
            Phone = (1 << 6) | Mobile,
            Continuum = (1 << 7) | Phone,
            Virtual = 1 << 8
        }

        [Flags]
        public enum DeviceFamilies
        {
            Unknown = 0,
            IoT = 1 << 0,
            Xbox = 1 << 1,
            Team = 1 << 2,
            HoloLens = 1 << 3,
            Desktop = 1 << 4,
            Mobile = 1 << 5
        }

        private DeviceUtils(Common.WindowWrapper windowWrapper)
        {
            MonitorUtils = MonitorUtils.Current(windowWrapper);
            WindowWrapper = windowWrapper ?? Common.WindowWrapper.Current();

            var di = windowWrapper.DisplayInformation();
            di.OrientationChanged += new Common.WeakReference<DeviceUtils, DisplayInformation, object>(this)
            {
                EventAction = (i, s, e) => i.Changed?.Invoke(i, EventArgs.Empty),
                DetachAction = (i, w) => di.OrientationChanged -= w.Handler
            }.Handler;

            var av = windowWrapper.ApplicationView();
            av.VisibleBoundsChanged += new Common.WeakReference<DeviceUtils, ApplicationView, object>(this)
            {
                EventAction = (i, s, e) => i.Changed?.Invoke(i, EventArgs.Empty),
                DetachAction = (i, w) => av.VisibleBoundsChanged -= w.Handler
            }.Handler;
        }

        #region singleton

        static Dictionary<Common.WindowWrapper, DeviceUtils> Cache = new Dictionary<Common.WindowWrapper, DeviceUtils>();
        public static DeviceUtils Current(Common.WindowWrapper windowWrapper = null)
        {
            windowWrapper = windowWrapper ?? Common.WindowWrapper.Current();
            if (!Cache.ContainsKey(windowWrapper))
            {
                var item = new DeviceUtils(windowWrapper);
                Cache.Add(windowWrapper, item);
                windowWrapper.ApplicationView().Consolidated += new Common.WeakReference<DeviceUtils, ApplicationView, object>(item)
                {
                    EventAction = (i, s, e) => Cache.Remove(windowWrapper),
                    DetachAction = (i, w) => windowWrapper.ApplicationView().Consolidated -= w.Handler
                }.Handler;
            }
            return Cache[windowWrapper];
        }

        #endregion

        public static DeviceFamilies CurrentDeviceFamily
        {
            get
            {
                var family = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
                switch (family)
                {
                    case "Windows.Desktop": return DeviceFamilies.Desktop;
                    case "Windows.Mobile": return DeviceFamilies.Mobile;
                    case "Windows.Team": return DeviceFamilies.Team;
                    case "Windows.IoT": return DeviceFamilies.IoT;
                    case "Windows.Xbox": return DeviceFamilies.Xbox;
                    case "Windows.Holographic": return DeviceFamilies.HoloLens;
                    default: return DeviceFamilies.Unknown;
                }
            }
        }

        public DeviceFamilies DeviceFamily() => CurrentDeviceFamily;

        public DeviceDispositions DeviceDisposition()
        {
            var x = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
            if (x.SystemProductName.Equals("Virtual"))
                return DeviceDispositions.Virtual;
            switch (DeviceFamily())
            {
                case DeviceFamilies.Desktop: return DeviceDispositions.Desktop;
                case DeviceFamilies.Team: return DeviceDispositions.Team;
                case DeviceFamilies.IoT: return DeviceDispositions.IoT;
                case DeviceFamilies.Xbox: return DeviceDispositions.Xbox;
                case DeviceFamilies.HoloLens: return DeviceDispositions.HoloLens;
                case DeviceFamilies.Mobile:
                    {
                        if (IsContinuum())
                            return DeviceDispositions.Continuum;
                        else if (IsPhone())
                            return DeviceDispositions.Phone;
                        else
                            return DeviceDispositions.Mobile;
                    }
                case DeviceFamilies.Unknown:
                default: return DeviceDispositions.Unknown;
            }
        }

        public bool IsTouch()
        {
            return WindowWrapper.UIViewSettings().UserInteractionMode == UserInteractionMode.Touch;
        }

        public bool IsPhone()
        {
            if (DeviceFamily() != DeviceFamilies.Mobile)
                return false;
            else
                return DiagonalSize() <= 7;

        }

        public bool IsContinuum()
        {
            if (DeviceFamily() != DeviceFamilies.Mobile)
                return false;
            if (IsTouch())
                return false;
            else
                return DiagonalSize() > 7;
        }

        public enum Units { Inches, Centimeters }

        public double DiagonalSize(Units units = Units.Inches)
        {
            var di = DisplayInformation.GetForCurrentView();
            var inches = 7;
            if (ApiInformation.IsPropertyPresent(typeof(DisplayInformation).ToString(), nameof(di.DiagonalSizeInInches)))
                return di.DiagonalSizeInInches.Value;
            switch (units)
            {
                case Units.Centimeters:
                    return inches * 2.54d;
                case Units.Inches:
                default:
                    return inches;
            }
        }
    }
}

