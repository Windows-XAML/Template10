using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Input;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace Template10.Utils
{
    public class MonitorUtils
    {
        public event EventHandler Changed;

        public InchesInfo Inches { get; private set; }

        public PixelsInfo Pixels { get; private set; }

        private MonitorUtils(Common.WindowWrapper windowWrapper)
        {
            var di = windowWrapper.DisplayInformation();
            di.OrientationChanged += new Common.WeakReference<MonitorUtils, DisplayInformation, object>(this)
            {
                EventAction = (i, s, e) => Changed?.Invoke(i, EventArgs.Empty),
                DetachAction = (i, w) => di.OrientationChanged -= w.Handler
            }.Handler;

            var av = windowWrapper.ApplicationView();
            av.VisibleBoundsChanged += new Common.WeakReference<MonitorUtils, ApplicationView, object>(this)
            {
                EventAction = (i, s, e) => Changed?.Invoke(i, EventArgs.Empty),
                DetachAction = (i, w) => av.VisibleBoundsChanged -= w.Handler
            }.Handler;

            Inches = new InchesInfo(windowWrapper);
            Pixels = new PixelsInfo(windowWrapper);
        }

        #region singleton

        static Dictionary<Common.WindowWrapper, MonitorUtils> Cache = new Dictionary<Common.WindowWrapper, MonitorUtils>();
        public static MonitorUtils Current(Common.WindowWrapper windowWrapper = null)
        {
            windowWrapper = windowWrapper ?? Common.WindowWrapper.Current();
            if (!Cache.ContainsKey(windowWrapper))
            {
                var item = new MonitorUtils(windowWrapper);
                Cache.Add(windowWrapper, item);
                windowWrapper.ApplicationView().Consolidated += new Common.WeakReference<MonitorUtils, ApplicationView, object>(item)
                {
                    EventAction = (i, s, e) => Cache.Remove(windowWrapper),
                    DetachAction = (i, w) => windowWrapper.ApplicationView().Consolidated -= w.Handler
                }.Handler;
            }
            return Cache[windowWrapper];
        }

        #endregion

        public class InchesInfo
        {
            Common.WindowWrapper WindowWrapper;
            public InchesInfo(Common.WindowWrapper windowWrapper)
            {
                windowWrapper = WindowWrapper;
            }

            public double Height
            {
                get
                {
                    var rect = PointerDevice.GetPointerDevices().Last().PhysicalDeviceRect;
                    return rect.Height / 96;
                }
            }

            public double Width
            {
                get
                {
                    var rect = PointerDevice.GetPointerDevices().Last().PhysicalDeviceRect;
                    return rect.Width / 96;
                }
            }

            public double Diagonal
            {
                get
                {
                    return Math.Sqrt(Math.Pow(Height, 2) + Math.Pow(Width, 2));
                }
            }
        }

        public class PixelsInfo
        {
            Common.WindowWrapper WindowWrapper;
            public PixelsInfo(Common.WindowWrapper windowWrapper)
            {
                windowWrapper = WindowWrapper;
            }

            public int Height
            {
                get
                {
                    var rect = PointerDevice.GetPointerDevices().Last().ScreenRect;
                    var scale = WindowWrapper.DisplayInformation().RawPixelsPerViewPixel;
                    return (int)(rect.Height * scale);
                }
            }

            public int Width
            {
                get
                {
                    var rect = PointerDevice.GetPointerDevices().Last().ScreenRect;
                    var scale = WindowWrapper.DisplayInformation().RawPixelsPerViewPixel;
                    return (int)(rect.Width * scale);
                }
            }

            public double Diagonal
            {
                get
                {
                    return Math.Sqrt(Math.Pow(Height, 2) + Math.Pow(Width, 2));
                }
            }
        }
    }
}
