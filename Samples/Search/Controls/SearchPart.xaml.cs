using System;
using System.Collections.ObjectModel;
using System.Linq;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.SearchSample.Controls
{
    public sealed partial class SearchPart : UserControl
    {
        public SearchPart()
        {
            InitializeComponent();
        }

        public event EventHandler HideRequested;
        public event TypedEventHandler<string> SelectionMade;

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            // raise event
            HideRequested?.Invoke(this, EventArgs.Empty);
        }

        private void SearchItemClicked(object sender, ItemClickEventArgs e)
        {
            // raise event
            SelectionMade?.Invoke(this, e.ClickedItem as string);
        }

        public ObservableCollection<string> Results { get; } = new ObservableCollection<string>();

        private void SearchClicked(object sender, RoutedEventArgs e)
        {
            Search();
        }

        public void Search()
        {
            Results.Clear();
            foreach (var item in Enumerable.Range(1, 10))
            {
                Results.Add(string.Format("Template10.Samples.SearchSample Result {0}", item));
            }
        }
    }
}
