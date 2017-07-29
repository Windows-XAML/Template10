using System;
using Template10.Controls;
using Template10.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class Busy : UserControl
    {
        public Busy()
        {
            InitializeComponent();
        }

        public string BusyText
        {
            get { return (string)GetValue(BusyTextProperty); }
            set { SetValue(BusyTextProperty, value); }
        }
        public static readonly DependencyProperty BusyTextProperty =
            DependencyProperty.Register(nameof(BusyText), typeof(string), typeof(Busy), new PropertyMetadata("Please wait..."));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(Busy), new PropertyMetadata(false));

        // hide and show busy dialog
        public static void ShowBusyFor(string text = null, int milliseconds = int.MaxValue)
        {
            DispatcherEx.Current().Dispatch(() =>
            {
                var modal = Window.Current.Content as ModalDialog;
                var view = modal.ModalContent as Busy;
                if (view == null)
                    modal.ModalContent = view = new Busy();
                modal.IsModal = view.IsBusy = true;
                view.BusyText = text;
            });
            DispatcherEx.Current().Dispatch(() =>
            {
                HideBusy();
            }, milliseconds);
        }

        public static void HideBusy()
        {
            DispatcherEx.Current().Dispatch(() =>
            {
                var modal = Window.Current.Content as ModalDialog;
                var view = modal.ModalContent as Busy;
                if (view != null)
                    modal.IsModal = view.IsBusy = false;
            });
        }
    }
}
