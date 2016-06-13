using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Popups;

namespace RateAndReview
{
    class RateReviewHelper
    {
        private Windows.Storage.ApplicationDataContainer localSettings;

        public RateReviewHelper()
        {
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        }

        public void LaunchRateAndReview(int count)
        {
            Retrieve(count);
        }

        private async void Retrieve(int count)
        {
            int x;

            var find = localSettings.Values["rrCount"];

            if (find == null)
            {
                localSettings.Values["rrCount"] = 1;

            }
            else
            {
                x = (int)localSettings.Values["rrCount"];                
               if (x <  count)
                {
                    x++;
                    localSettings.Values["rrCount"] = x;
                }
                if (x == count)
                {
                    x++;
                    localSettings.Values["count"] = x;
                    MessageDialog dialog = new MessageDialog("Do you want to rate and review the app?", "Review");
                    dialog.Commands.Add(new UICommand("Okay", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                    dialog.Commands.Add(new UICommand("Later", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;
                    await dialog.ShowAsync();

                } 
            }
        }

        private async void CommandInvokedHandler(IUICommand command)
        {

            if (command.Label == "Okay")
            {
                await Launch();
            }
            else
            {
                localSettings.Values.Remove("rrCount");
            }
        }

        private async Task Launch()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;

            try
            {
                await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductID=" + packageId.ProductId, UriKind.RelativeOrAbsolute));
            }

           catch(InvalidCastException ex)
            {
                Debug.WriteLine( "\n\n No ProductID found. The app should be on store to go to the Rate and Review page.\n\n");
            }           
        }
    }
}
