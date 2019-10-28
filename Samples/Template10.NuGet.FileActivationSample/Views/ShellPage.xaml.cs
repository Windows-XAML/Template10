using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Prism.Ioc;
using Template10.Navigation;
using Template10.NuGet.FileActivationSample.ViewModels;
using Template10.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Template10.NuGet.FileActivationSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        private IDialogService _dialogService;
        public event EventHandler<Exception> NavigationFailed;

        public ShellPage(IDialogService dialogService)
        {
            InitializeComponent();
            _dialogService = dialogService;
        }

        public new Frame Frame
        {
            get => MainNavigationView.Content as Frame;
            set => MainNavigationView.Content = value;
        }

        private async void MainNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                await _dialogService.AlertAsync("Navigate to settings here");
            }
            else
            {
                switch (args.InvokedItemContainer)
                {
                    case NavigationViewItem item when Equals(item, HomeNavigationViewItem):
                        {
                            await NavigateAsync(nameof(MainPage));
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private async Task NavigateAsync(string key)
        {
            var navigation = Frame.GetNavigationService();
            var result = await navigation.NavigateAsync(key);
            if (result.Success)
            {
                return;
            }
            NavigationFailed?.Invoke(Frame, result.Exception);
        }
    }
}
