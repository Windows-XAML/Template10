using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Prism.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Sample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage() => InitializeComponent();

        ViewModels.MainPageViewModel ViewModel => DataContext as ViewModels.MainPageViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.TryGetParameter<int>("Record", out var value))
            {
                // ViewModel.HeaderText = $"Record: {value}";
            }
            else
            {
                // handle failure
            }
        }
    }
}
