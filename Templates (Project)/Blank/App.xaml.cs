﻿using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;

namespace Template10
{
    sealed partial class App : Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            this.SplashFactory = (e) => null;
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // start the user experience
            NavigationService.Navigate(typeof(Views.MainPage), "123");
            return Task.FromResult<object>(null);
        }
    }
}
