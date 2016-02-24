﻿using Windows.UI.Xaml;

namespace Messaging.ViewModels
{
    public class ViewModelLocator
    {
        private MainPageViewModel _MainPageViewModel;
        public MainPageViewModel MainPageViewModel =>
                _MainPageViewModel ?? (_MainPageViewModel = new MainPageViewModel());

        private DetailPageViewModel _DetailPageViewModel;
        public DetailPageViewModel DetailPageViewModel =>
                _DetailPageViewModel ?? (_DetailPageViewModel = new DetailPageViewModel());
    }
}
