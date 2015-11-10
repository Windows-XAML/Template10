using System.Linq;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Controls
{
    public sealed partial class SearchPart : UserControl
    {
        public SearchPart()
        {
            this.InitializeComponent();
        }

        public object NormalVisualState { get; }

        private void SearchSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!e.AddedItems.Any())
                return;
            BootStrapper.Current.NavigationService.Navigate(typeof(Views.DetailPage), e.AddedItems.FirstOrDefault());
        }
    }
}
