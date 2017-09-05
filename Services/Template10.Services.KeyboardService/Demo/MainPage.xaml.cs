using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Services.KeyboardService;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Template10.Demo.KeyboardServiceDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs args)
        {
            var s = KeyboardService.Instance;
            s.AfterKeyDown = e => MyListView.Items.Insert(0, e.ToString());
        }
    }
}
