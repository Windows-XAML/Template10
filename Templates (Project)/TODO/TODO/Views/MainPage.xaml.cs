using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = this.DataContext as ViewModels.MainPageViewModel;
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar= true;
            Window.Current.SetTitleBar(AppTitle);
        }

        ViewModels.MainPageViewModel ViewModel { get; set; }

        private async void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            this.TodoEditorDialog.DataContext = e.ClickedItem;
            await this.TodoEditorDialog.ShowAsync();
        }

        private async void List_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var data = textBlock.DataContext as ViewModels.TodoListViewModel;
            this.ListEditorDialog.DataContext = data.TodoList;
            this.ListEditorDialog.SecondaryButtonCommand = this.ViewModel.RemoveListCommand;
            this.ListEditorDialog.SecondaryButtonCommandParameter = data;
            await this.ListEditorDialog.ShowAsync();
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
