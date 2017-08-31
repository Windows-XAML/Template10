using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public ViewModels.MainPageViewModel ViewModel
            => DataContext as ViewModels.MainPageViewModel;
    }

    class User { }

    class MyClass01
    {
        IList<User> others;
        public string Count1()
          => $"Including just you: {others.Count + 1} users";
        public string Count2()
          => $"Including you & me: {others.Count + 2} users";
    }

    class MyClass02
    {
        IList<User> others;
        public string Count1()
          => $"Including just you: {Add(others.Count, 1)} users";
        public string Count2()
          => $"Including you & me: {Add(others.Count, 2)} users";
        public double Add(double left, double right)
          => left + right;
    }


}