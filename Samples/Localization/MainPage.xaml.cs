using Windows.UI.Xaml.Controls;
using Template10.Service.LocalizationService.Services.LocalizationService;
using System;
using System.Linq;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LocalizationSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        LocalizationService _localizationService;
        public MainPage()
        {
            this.InitializeComponent();
            _localizationService = new LocalizationService();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LanguageComboBox.ItemsSource = _localizationService.SupportedLanguages;
            LanguageComboBox.SelectedIndex = Array.IndexOf(_localizationService.SupportedLanguages.ToArray(), _localizationService.CurrentLanguage);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(LanguageComboBox.SelectedItem as string))
                return;

            _localizationService.SetLocale((string)LanguageComboBox.SelectedItem);

            //clear BackStack
            Frame.Navigate(Frame.CurrentSourcePageType);
            Frame.BackStack.Clear();
        }
    }
}
