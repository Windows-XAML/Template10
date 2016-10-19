﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.BehaviorsSample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public int ThrottledEventCount
        {
            get { return (int) GetValue(ThrottledEventCountProperty); }
            set { SetValue(ThrottledEventCountProperty, value); }
        }

        public static readonly DependencyProperty ThrottledEventCountProperty = DependencyProperty.Register(nameof(ThrottledEventCount), typeof (int), typeof (MainPage), new PropertyMetadata(0));

        public void IncreaseThrottledEventCount()
        {
            ThrottledEventCount = ThrottledEventCount + 1;
        }
    }
}
