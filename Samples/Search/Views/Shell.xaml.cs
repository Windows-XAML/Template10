﻿using System;
using System.Linq;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        private static WindowWrapper Window { get; set; }
        public Shell(NavigationService navigationService)
        {
            Instance = this;
            this.InitializeComponent();

            // setup for static calls
            Window = WindowWrapper.Current();
            MyHamburgerMenu.NavigationService = navigationService;

            // any nav change, reset to normal
            navigationService.FrameFacade.Navigated += (s, e) =>
                BusyModal.IsModal = SearchModal.IsModal = LoginModal.IsModal = false;
        }

        #region Busy

        public static void ShowBusy(bool busy, string text = null)
        {
            Window.Dispatcher.Dispatch(() =>
            {
                Instance.BusyText.Text = text ?? string.Empty;
                Instance.BusyModal.IsModal = busy;
            });
        }

        #endregion

        #region Login

        private void LoginTapped(object sender, RoutedEventArgs e)
        {
            LoginModal.IsModal = true;
        }

        private void LoginHide(object sender, System.EventArgs e)
        {
            LoginButton.IsEnabled = true;
            LoginModal.IsModal = false;
        }

        private void LoginLoggedIn(object sender, EventArgs e)
        {
            LoginButton.IsEnabled = false;
            LoginModal.IsModal = false;
        }

        #endregion

        #region Search

        private void SearchTapped(object sender, RoutedEventArgs e)
        {
            SearchModal.IsModal = true;
        }

        // request to hide search (from inside search)
        private void SearchHide(object sender, EventArgs e)
        {
            SearchModal.IsModal = false;
        }

        // request to goto detail
        private void SearchNav(object sender, string item)
        {
            SearchModal.IsModal = false;
            MyHamburgerMenu.NavigationService.Navigate(typeof(Views.DetailPage), item);
        }

        #endregion


    }
}
