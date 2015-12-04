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

        public enum DeviceDispositions { Unknown, IoT, Xbox, Team, Desktop, Mobile, Phone, Continuum }

        public enum DeviceFamilies { Unknown, Desktop, Mobile, Team, IoT, Xbox }

        private DeviceUtils(Common.WindowWrapper windowWrapper)
        {
            MonitorUtils = Utils.MonitorUtils.Current(windowWrapper);

            var di = windowWrapper.DisplayInformation();
            di.OrientationChanged += new Common.WeakReference<DeviceUtils, DisplayInformation, object>(this)
            {
                EventAction = (i, s, e) => Changed?.Invoke(i, EventArgs.Empty),
                DetachAction = (i, w) => di.OrientationChanged -= w.Handler
            }.Handler;

            var av = windowWrapper.ApplicationView();
            av.VisibleBoundsChanged += new Common.WeakReference<DeviceUtils, ApplicationView, object>(this)
            {
                EventAction = (i, s, e) => Changed?.Invoke(i, EventArgs.Empty),
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

        public DeviceFamilies DeviceFamily()
        {
            var family = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
            switch (family)
            {
                case "Windows.Desktop": return DeviceFamilies.Desktop;
                case "Windows.Mobile": return DeviceFamilies.Mobile;
                case "Windows.Team": return DeviceFamilies.Team;
                case "Windows.IoT": return DeviceFamilies.IoT;
                case "Windows.Xbox": return DeviceFamilies.Xbox;
                default: return DeviceFamilies.Unknown;
            }
        }

        public DeviceDispositions DeviceDisposition()
        {
            switch (DeviceFamily())
            {
                case DeviceFamilies.Desktop: return DeviceDispositions.Desktop;
                case DeviceFamilies.Team: return DeviceDispositions.Team;
                case DeviceFamilies.IoT: return DeviceDispositions.IoT;
                case DeviceFamilies.Xbox: return DeviceDispositions.Xbox;
                case DeviceFamilies.Mobile:
                    {
                        if (!IsTouch())
                        {
                            return DeviceDispositions.Continuum;
                        }
                        else
                        {
                            if (MonitorUtils.Inches.Diagonal > 6)
                                return DeviceDispositions.Mobile;
                            return DeviceDispositions.Phone;
                        }
                    }
                case DeviceFamilies.Unknown:
                default: return DeviceDispositions.Unknown;
            }
        }

        public bool IsTouch()
        {
            return WindowWrapper.UIViewSettings().UserInteractionMode == UserInteractionMode.Touch;
        }

        public bool IsContinuum()
        {
            if (DeviceFamily() != DeviceFamilies.Mobile)
                return false;
            // This will be supported in 10.0.10563.0
            var inches = 7; // WindowWrapper.DisplayInformation().DiagonalSizeInInches;
            if (inches > 6)
                return true;
            return false;
        }
    }
}
