using Windows.UI.Xaml.Controls;

namespace Template10.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = this.DataContext as ViewModels.MainPageViewModel;
            Loaded += (s, e) => Windows.UI.Xaml.Window.Current.SizeChanged += Current_SizeChanged;
            Unloaded += (s, e)=> Windows.UI.Xaml.Window.Current.SizeChanged -= Current_SizeChanged; 
        }

        ViewModels.MainPageViewModel ViewModel { get; set; }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private async void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            this.TodoEditorDialog.DataContext = e.ClickedItem;
            this.TodoEditorDialog.ShowAsync();
        }

        private void TextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.Key == Windows.System.VirtualKey.Enter 
                && !string.IsNullOrEmpty(textBox.Text)
                && textBox.Text.Length > 3)
            {
                e.Handled = true;
                var list = textBox.DataContext as ViewModels.TodoListViewModel;
                list.AddCommand.Execute(textBox.Text);
                textBox.Text = string.Empty;
                textBox.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }
    }
}
