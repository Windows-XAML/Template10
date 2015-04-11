using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;

namespace Template10.Services.SecondaryTileService
{
    public class SecondaryTileInfo
    {
        public TileVisualElements VisualElements = new TileVisualElements();
        public FrameworkElement AnchorElement { get; set; }
        public Windows.UI.Popups.Placement RequestPlacement { get; set; }
        public string Arguments { get; set; }
        public string DisplayName { get; set; }
        public string PhoneticName { get; set; }
        public bool LockScreenDisplayBadgeAndTileText { get; set; }
        public Uri LockScreenBadgeLogo { get; set; }
        public class TileVisualElements
        {
            public Windows.UI.Color BackgroundColor { get; set; }
            public ForegroundText ForegroundText { get; set; }
            public bool ShowNameOnSquare150x150Logo { get; set; }
            public bool ShowNameOnSquare310x310Logo { get; set; }
            public bool ShowNameOnWide310x150Logo { get; set; }
            public Uri Square150x150Logo { get; set; }
            public Uri Square30x30Logo { get; set; }
            public Uri Square310x310Logo { get; set; }
            public Uri Wide310x150Logo { get; set; }
        }
        public Rect Rect()
        {
            if (this.AnchorElement == null)
                return new Rect(0, 0, 0, 0);
            var transform = this.AnchorElement.TransformToVisual(null);
            var point = transform.TransformPoint(new Point());
            var size = new Size(this.AnchorElement.ActualWidth, this.AnchorElement.ActualHeight);
            return new Rect(point, size);
        }
    }
}
