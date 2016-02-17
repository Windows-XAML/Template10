using System;
using Sample.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

namespace Sample.Views
{
    public class x
    {
        public static ObservableCollection<string> Items { get; } = new ObservableCollection<string>();
    }

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<string> Items { get { return x.Items; } }
    }
}