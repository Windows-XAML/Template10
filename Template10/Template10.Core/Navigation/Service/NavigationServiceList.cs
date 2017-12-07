using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Template10.Navigation
{
    public class NavigationServiceList : List<INavigationService>
    {
        internal NavigationServiceList() { }
        public INavigationService GetByFrameId(string frameId) => this.FirstOrDefault(x => x.FrameEx.FrameId == frameId);
        public INavigationService GetByFrameFacade(IFrameEx frame) => this.FirstOrDefault(x => x.FrameEx.Equals(frame));
        public INavigationService GetByFrame(Frame frame) => this.FirstOrDefault(x => (x.FrameEx as IFrameEx2).Frame.Equals(frame));
    }
}
