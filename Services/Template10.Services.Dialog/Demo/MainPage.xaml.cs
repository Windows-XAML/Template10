using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Template10.Services.Dialog;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Demo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DialogService _dialog;

        public MainPage()
        {
            this.InitializeComponent();
            _dialog = new DialogService();
        }

        public string OutputText
        {
            get { return (string)GetValue(OutputTextProperty); }
            set { SetValue(OutputTextProperty, value); }
        }
        public static readonly DependencyProperty OutputTextProperty =
            DependencyProperty.Register("OutputText", typeof(string),
                typeof(MainPage), new PropertyMetadata("Click a button."));

        void Log(string message)
        {
            OutputText = message + Environment.NewLine + (OutputText ?? string.Empty);
        }

        private async void Alert(object sender, RoutedEventArgs e)
        {
            Log("Showing alert");
            var resolver = UseCustomResourceResolver.IsChecked.Value ? new CustomResolver() : null;
            var result = await _dialog.AlertAsync(AlertTextBox.Text, resolver);
            Log($"Alert result: {result}");
        }

        private async void PromptYesNo(object sender, RoutedEventArgs e)
        {
            Log("Showing prompt(YesNo)");
            var resolver = UseCustomResourceResolver.IsChecked.Value ? new CustomResolver() : null;
            var result = await _dialog.PromptAsync(AlertTextBox.Text, MessageBoxType.YesNo , resolver);
            Log($"Prompt(YesNo) result: {result}");
        }

        private async void PromptYesNoCancel(object sender, RoutedEventArgs e)
        {
            Log("Showing prompt(YesNoCancel)");
            var resolver = UseCustomResourceResolver.IsChecked.Value ? new CustomResolver() : null;
            var result = await _dialog.PromptAsync(AlertTextBox.Text, MessageBoxType.YesNoCancel, resolver);
            Log($"Prompt(YesNoCancel) result: {result}");
        }

        private void Splash(object sender, RoutedEventArgs e)
        {

        }

        private void Busy(object sender, RoutedEventArgs e)
        {

        }
    }

    public class CustomResolver : IResourceResolver
    {
        public string Resolve(ResourceTypes resource)
        {
            switch (resource)
            {
                case ResourceTypes.Ok:
                    return "Cat";
                case ResourceTypes.Yes:
                    return "Dog";
                case ResourceTypes.No:
                    return "Bird";
                case ResourceTypes.Cancel:
                    return "Horse";
                default:
                    throw new NotSupportedException($"{resource}");
            }
        }
    }
}
