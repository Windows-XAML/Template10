using System;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class BusyPopupData : PopupDataBase
    {
        public BusyPopupData() : base(null, null)
        {
            // invalid
        }

        public BusyPopupData(Action close, CoreDispatcher dispatcher) : base(close, dispatcher)
        {
            // empty
        }
    }

    [ContentProperty(Name = nameof(Template))]
    public class BusyPopup : PopupItemBase<BusyPopupData>
    {
        public override void Initialize()
        {
            // empty
        }
    }
}