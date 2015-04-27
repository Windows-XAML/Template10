using Windows.UI.Xaml.Controls;

namespace Template10.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += (s, e) => Windows.UI.Xaml.Window.Current.SizeChanged += Current_SizeChanged;
            Unloaded += (s, e)=> Windows.UI.Xaml.Window.Current.SizeChanged -= Current_SizeChanged; 
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private async void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            this.TodoEditorDialog.DataContext = e.ClickedItem;
            this.TodoEditorDialog.ShowAsync();
        }
    }
}
