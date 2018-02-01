using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Prism.Windows.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PrismSample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += (s, e) => SetupVerticalScrollBar();
        }

        private void SetupVerticalScrollBar()
        {
            var children = Prism.Windows.Utilities.XamlUtilities.RecurseChildren(MainGridView);
            var child = children.Single(x => x.Name == "VerticalScrollBar") as ScrollBar;
            child.Margin = new Thickness(0, 48, 0, 0);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.TryGetParameter<int>("Record", out var value))
            {
                HeaderTextBlock.Text = $"Record: {value}";
            }
            SubHeaderTextBlock.Text = DateTime.Now.ToLongTimeString();
        }
    }
}
