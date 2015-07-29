﻿using Template10.ViewModels;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace Template10.Views
{
    public sealed partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        // strongly-typed view models enable x:bind
        public Page2ViewModel ViewModel { get { return DataContext as Page2ViewModel; } }
    }
}
