using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.NavigationService
{
    public class NavigationServiceList : List<INavigationService>
    {
        internal NavigationServiceList() { }
        public INavigationService GetByFrameId(string frameId) => this.FirstOrDefault(x => x.FrameFacade.FrameId == frameId);
        public INavigationService GetByFrameFacade(IFrameWrapper frame) => this.FirstOrDefault(x => x.FrameFacade.Equals(frame));
        public INavigationService GetByFrame(Frame frame) => this.FirstOrDefault(x => (x.FrameFacade as IFrameWrapperInternal).Frame.Equals(frame));
    }
}
