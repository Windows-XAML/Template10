using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Sample.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        public DetailPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
            }
        }


        private double pageWidth;
        public double PageWidth
        {
            get { return pageWidth; }
            set
            {
                pageWidth = value;
                RaisePropertyChanged(nameof(PageWidth));
            }
        }

        private bool isMenuPaneOpened;
        public bool IsMenuPaneOpened
        {
            get { return isMenuPaneOpened; }
            set
            {
                isMenuPaneOpened = value;
                RaisePropertyChanged(nameof(IsMenuPaneOpened));
            }
        }

        private double appBarButtonWidth;
        public double AppBarButtonWidth
        {
            get { return appBarButtonWidth; }
            set
            {
                appBarButtonWidth = value;
                RaisePropertyChanged(nameof(AppBarButtonWidth));
            }
        }

        private void Opened_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IsMenuPaneOpened = true;
        }

        private void Closed_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IsMenuPaneOpened = false;
        }

        public void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            PageWidth = e.NewSize.Width;
            SetDimensions(e.NewSize.Width, e.NewSize.Height);
        }

        private void SetDimensions(double width, double height)
        {

            if (width <= 320)
            {
                AppBarButtonWidth = 50;
            }
            else if (width <= 360) /* 341 and 360 */
            {
                AppBarButtonWidth = 56;
            }
            else if (width <= 411)
            {
                AppBarButtonWidth = 60;
            }
            else if (width <= 521)
            {
                AppBarButtonWidth = 68;
            }
            else
            {
                AppBarButtonWidth = 100;
            }
        }
    }
}
