using System;
using System.Linq;
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
            this.DataContextChanged += (s, e) => { this.ViewModel = this.DataContext as ViewModels.MainPageViewModel; };

            // extend the app into the title for a custom look
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(AppTitle);
        }

        // strongly-typed view models enable x:bind
        ViewModels.MainPageViewModel ViewModel { get; set; }

        private async void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            var list = this.ViewModel.TodoLists.First(x => x.Items.Contains(e.ClickedItem));
            var editor = new Controls.TodoItemEditor
            {
                PrimaryButtonText = "Close",
                SecondaryButtonText = "Delete",
                DataContext = e.ClickedItem,
                SecondaryButtonCommand = list.RemoveCommand,
                SecondaryButtonCommandParameter = e.ClickedItem,
            };
            await editor.ShowAsync();
        }

        private async void List_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var data = textBlock.DataContext as ViewModels.TodoListViewModel;

            var editor = new Controls.TodoListEditor
            {
                PrimaryButtonText = "Close",
                SecondaryButtonText = "Delete",
                DataContext = data,
                SecondaryButtonCommand = this.ViewModel.RemoveListCommand,
                SecondaryButtonCommandParameter = data,
            };
            await editor.ShowAsync();
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
