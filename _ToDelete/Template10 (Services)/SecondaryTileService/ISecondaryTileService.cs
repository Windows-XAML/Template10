using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;

namespace Template10.Services.SecondaryTileService
{
    public interface ISecondaryTileService
    {
        Task<bool> PinAsync(TileInfo info, string tileId, string arguments);
        bool Exists(string tileId);
        Task<bool> UnpinAsync(string tileId, Rect selection, Windows.UI.Popups.Placement placement = Windows.UI.Popups.Placement.Above);
        Task<bool> UnpinAsync(string tileId);
    }

    public class TileInfo
    {
        public FrameworkElement AnchorElement { get; set; }
        public Windows.UI.Popups.Placement RequestPlacement { get; set; }

        public string Arguments { get; set; }
        public string DisplayName { get; set; }
        public string PhoneticName { get; set; }
        public bool LockScreenDisplayBadgeAndTileText { get; set; }
        public Uri LockScreenBadgeLogo { get; set; }

        public TileVisualElements VisualElements = new TileVisualElements();
        public class TileVisualElements
        {
            public Windows.UI.Color BackgroundColor { get; set; }
            public ForegroundText ForegroundText { get; set; }
            public bool ShowNameOnSquare150x150Logo { get; set; }
            public bool ShowNameOnSquare310x310Logo { get; set; }
            public bool ShowNameOnWide310x150Logo { get; set; }
            public Uri Square150x150Logo { get; set; }
            public Uri Square310x310Logo { get; set; }
            public Uri Wide310x150Logo { get; set; }
        }

        public Rect Rect()
        {
            if (this.AnchorElement == null)
                return new Rect();
            var transform = this.AnchorElement.TransformToVisual(null);
            var point = transform.TransformPoint(new Point());
            var size = new Size(this.AnchorElement.ActualWidth, this.AnchorElement.ActualHeight);
            return new Rect(point, size);
        }
    }
}
