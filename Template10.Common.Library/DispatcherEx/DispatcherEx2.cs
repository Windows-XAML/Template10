using Windows.UI.Core;

namespace Template10.Common
{
    public partial class DispatcherEx : IDispatcherEx2
    {
        IDispatcherEx2 Two => this as IDispatcherEx2;

        CoreDispatcher IDispatcherEx2.CoreDispatcher { get; set; }
    }
}
