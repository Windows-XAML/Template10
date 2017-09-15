using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class NarrowPopupData : PopupDataBase
    {
        internal NarrowPopupData(Action close, CoreDispatcher dispatcher) : base(close, dispatcher)
        {
            // empty
        }
    }

    [ContentProperty(Name = nameof(Template))]
    public class NarrowPopup : PopupItemBase<NarrowPopupData>
    {
        public double SmallerThan { get; set; } = 420d;

        public override void Initialize()
        {
            Window.Current.SizeChanged += (s, e) => IsShowing = (e.Size.Width < SmallerThan);
        }
    }
}