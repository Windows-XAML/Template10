using System;
using System.Collections.Generic;
using Template10.Common;
using Template10.Core;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace Template10.Extensions
{
    public class MonitorUtils
    {
        public event EventHandler Changed;

        public InchesInfo Inches { get; }

        public PixelsInfo Pixels { get; }

        private MonitorUtils(IWindowEx windowWrapper)
        {
            var di = windowWrapper.DisplayInformation;
            di.OrientationChanged += new WeakReference<MonitorUtils, DisplayInformation, object>(this)
            {
                EventAction = (i, s, e) => i.Changed?.Invoke(i, EventArgs.Empty),
                DetachAction = (i, w) => di.OrientationChanged -= w.Handler
            }.Handler;

            var av = windowWrapper.ApplicationView;
            av.VisibleBoundsChanged += new WeakReference<MonitorUtils, ApplicationView, object>(this)
            {
                EventAction = (i, s, e) => i.Changed?.Invoke(i, EventArgs.Empty),
                DetachAction = (i, w) => av.VisibleBoundsChanged -= w.Handler
            }.Handler;

            Inches = new InchesInfo(windowWrapper);
            Pixels = new PixelsInfo(windowWrapper);
        }

        #region singleton

        private static Dictionary<IWindowEx, MonitorUtils> Cache = new Dictionary<IWindowEx, MonitorUtils>();

        public static MonitorUtils Current(IWindowEx windowWrapper = null)
        {
            windowWrapper = windowWrapper ?? throw new ArgumentNullException(nameof(windowWrapper));
            if (!Cache.ContainsKey(windowWrapper))
            {
                var item = new MonitorUtils(windowWrapper);
                Cache.Add(windowWrapper, item);
                windowWrapper.ApplicationView.Consolidated += new WeakReference<MonitorUtils, ApplicationView, object>(item)
                {
                    EventAction = (i, s, e) => Cache.Remove(windowWrapper),
                    DetachAction = (i, w) => windowWrapper.ApplicationView.Consolidated -= w.Handler
                }.Handler;
            }
            return Cache[windowWrapper];
        }

        public void Maximize()
        {
            var size = new Windows.Foundation.Size(Current().Pixels.Width, Current().Pixels.Height);
            size.Height -= 100;
            size.Width -= 100;
            var av = ApplicationView.GetForCurrentView();
            av.TryResizeView(size);
        }

        #endregion singleton

        public class InchesInfo
        {
            private IWindowEx WindowWrapper;

            public InchesInfo(IWindowEx windowWrapper)
            {
                WindowWrapper = windowWrapper;
            }

            public double Height => Current().Pixels.Height / 96;

            public double Width => Current().Pixels.Width / 96;

            public double Diagonal => Math.Sqrt(Math.Pow(Height, 2) + Math.Pow(Width, 2));
        }

        public class PixelsInfo
        {
            private IWindowEx WindowWrapper;

            public PixelsInfo(IWindowEx windowWrapper)
            {
                WindowWrapper = windowWrapper;
            }

            public double Height
            {
                get
                {
                    var av = ApplicationView.GetForCurrentView();
                    var bounds = av.VisibleBounds;
                    var di = DisplayInformation.GetForCurrentView();
                    var factor = di.RawPixelsPerViewPixel;
                    return bounds.Height * factor;
                }
            }

            public double Width
            {
                get
                {
                    var av = ApplicationView.GetForCurrentView();
                    var bounds = av.VisibleBounds;
                    var di = DisplayInformation.GetForCurrentView();
                    var factor = di.RawPixelsPerViewPixel;
                    return bounds.Width * factor;
                }
            }

            public double Diagonal => Math.Sqrt(Math.Pow(Height, 2) + Math.Pow(Width, 2));
        }
    }
}