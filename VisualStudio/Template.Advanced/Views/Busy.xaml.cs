using System;
using Template10.Controls;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class Busy : UserControl
    {
        public Busy()
        {
            InitializeComponent();
            Loaded += (s, e) => IsBusy = true;
            Unloaded += (s, e) => IsBusy = false;
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(nameof(IsBusy), typeof(bool),
                typeof(Busy), new PropertyMetadata(false));

        public string BusyText
        {
            get { return (string)GetValue(BusyTextProperty); }
            set { SetValue(BusyTextProperty, value); }
        }
        public static readonly DependencyProperty BusyTextProperty =
            DependencyProperty.Register(nameof(BusyText), typeof(string),
                typeof(Busy), new PropertyMetadata("DEFAULT_STRING"));
    }
}
