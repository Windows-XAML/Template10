using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Demo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        enum Operations { Add, Subtract }
        Operations operation = Operations.Add;

        private void Timer_Tick(object sender, object e)
        {
            if (MyListView.SelectedIndex == 0)
            {
                MyListView.SelectedIndex++;
                operation = Operations.Add;
            }
            else if (MyListView.SelectedIndex == MyListView.Items.Count - 1)
            {
                MyListView.SelectedIndex--;
                operation = Operations.Subtract;
            }
            else if (operation == Operations.Add)
            {
                MyListView.SelectedIndex++;
            }
            else if (operation == Operations.Subtract)
            {
                MyListView.SelectedIndex--;
            }
        }
    }
}
