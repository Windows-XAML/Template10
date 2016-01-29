using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
            DependencyProperty.Register("BusyText", typeof(string), typeof(Busy), new PropertyMetadata("Please wait..."));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(Busy), new PropertyMetadata(false));
    }
}
