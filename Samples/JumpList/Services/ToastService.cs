using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpList.Services
{
    public class ToastService
    {
        public void ShowToast(Models.FileInfo file, string message = "Success")
        {
            var image = "https://raw.githubusercontent.com/Windows-XAML/Template10/master/Assets/Template10.png";

            var content = new NotificationsExtensions.Toasts.ToastContent()
            {
                Launch = file.Ref.Path,
                Visual = new NotificationsExtensions.Toasts.ToastVisual()
                {
                    TitleText = new NotificationsExtensions.Toasts.ToastText()
                    {
                        Text = message
                    },

                    BodyTextLine1 = new NotificationsExtensions.Toasts.ToastText()
                    {
                        Text = file.Name
                    },

                    AppLogoOverride = new NotificationsExtensions.Toasts.ToastAppLogo()
                    {
                        Crop = NotificationsExtensions.Toasts.ToastImageCrop.Circle,
                        Source = new NotificationsExtensions.Toasts.ToastImageSource(image)
                    }
                },
                Audio = new NotificationsExtensions.Toasts.ToastAudio()
                {
                    Src = new Uri("ms-winsoundevent:Notification.IM")
                }
            };

            var notification = new Windows.UI.Notifications.ToastNotification(content.GetXml());
            var notifier = Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier();
            notifier.Show(notification);
        }

    }
}
