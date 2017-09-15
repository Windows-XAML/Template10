using System;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Markup;

namespace Template10.Popups
{
    public class BusyPopupData : PopupDataBase
    {
        public BusyPopupData(Action close, CoreDispatcher dispatcher) : base(close, dispatcher)
        {
            // empty
        }

        private bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set => RaisePropertyChanged(() => _isActive = value);
        }
    }

    [ContentProperty(Name = nameof(Template))]
    public class BusyPopup : PopupItemBase<BusyPopupData>
    {
        public override void Initialize()
        {
            // empty
        }

        public new bool IsShowing
        {
            get => base.IsShowing;
            set => base.IsShowing = Content.IsActive = value;
        }
    }
}