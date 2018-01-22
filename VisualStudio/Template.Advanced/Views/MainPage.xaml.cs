using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContextChanged += (s, e) => Bindings.Update();
        }

        public ViewModels.MainPageViewModel ViewModel
            => DataContext as ViewModels.MainPageViewModel;
    }
}