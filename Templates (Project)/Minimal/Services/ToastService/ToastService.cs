using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Notifications;

namespace Minimal.Services.ToastService
{
    public class ToastService
    {
        public async Task ShowAsync(string title, string text)
        {
            try
            {
                // prep for custom
                var folder = await Windows.ApplicationModel.Package.Current
                   .InstalledLocation.GetFolderAsync("Services");
                folder = await folder.GetFolderAsync("ToastService");
                var file = await folder.GetFileAsync("SimpleToast.xml");
                var xml = XDocument.Load(file.Path);

                // fetch targets
                var imageElement = xml.Descendants("image").First();
                var titleElement = xml.Descendants("text").First();
                var subtitleElement = xml.Descendants("text").ToArray()[1];

                // set values
                var imagePath = new Uri("ms-appx://Services/ToastService/Template10.png");
                imageElement.SetAttributeValue("src", imagePath.ToString());
                titleElement.SetValue(title);
                subtitleElement.SetValue(text);

                // show
                var toastNotification = new ToastNotification(xml.ToXmlDocument());
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                toastNotifier.Show(toastNotification);
            }
            catch (Exception)
            {
                System.Diagnostics.Debugger.Break();
            }
        }
    }
}
