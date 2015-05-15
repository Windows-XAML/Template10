using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Template10.Controls
{
    public sealed partial class BackButton : UserControl
    {
        public BackButton()
        {
            this.InitializeComponent();
            Loaded += BackButton_Loaded;
            MyBackButton.Click += (s, e) => Frame.GoBack();
        }

        public Frame Frame { get; private set; }

        private void BackButton_Loaded(object sender, RoutedEventArgs e)
        {
            DependencyObject item = this;
            while (!((item = VisualTreeHelper.GetParent(item)) is Page)) { }
            Page page = item as Page;
            this.Frame = page.Frame;
            this.Visibility = CalculateOnCanvasBackVisibility();
            Window.Current.SizeChanged += (s, arg) => this.Visibility = CalculateOnCanvasBackVisibility();
        }

        private Visibility CalculateOnCanvasBackVisibility()
        {
            var cangoback = Frame.CanGoBack;
            if (!cangoback)
                return Visibility.Collapsed;

            var mobilefam = ResourceContext.GetForCurrentView().QualifierValues["DeviceFamily"].Equals("Mobile");
            if (mobilefam)
                return Visibility.Collapsed;

            var desktopfam = ResourceContext.GetForCurrentView().QualifierValues["DeviceFamily"].Equals("Desktop");
            if (!desktopfam)
                return Visibility.Collapsed;

            var touchmode = UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Touch;
            if (touchmode)
                return Visibility.Collapsed;

            var fullscreen = ApplicationView.GetForCurrentView().IsFullScreen;
            if (fullscreen)
                return Visibility.Visible;

            var optinback = SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility.Equals(AppViewBackButtonVisibility.Visible);
            if (optinback)
            {
                var hastitle = !CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar;
                if (hastitle)
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }
    }
}
