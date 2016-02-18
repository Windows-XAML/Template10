using System;

namespace Template10.Services.ToastService
{

    // http://msdn.microsoft.com/en-us/library/windows/apps/Hh761494.aspx

    /* In this set of templates,
    * the image element is expressed using one of these protocols:
    * http:// or https:// - a web-based image.
    * ms-appx:/// - an image included in the app package.
    * ms-appdata:///local/ - an image saved to local storage.
    * file:/// - a local image. (Only supported for desktop apps.)
    */

    public class ToastService : IToastService
    {
        ToastHelper _helper = new ToastHelper();

        public void ShowToastText01(string content, string arg = null)
        {
            var toast = ToastContent.CreateToastText01(content, arg);
            _helper.ShowToast(toast);
        }

        public void ShowToastText02(string title, string content, string arg = null)
        {
            var toast = ToastContent.CreateToastText02(title, content, arg);
            _helper.ShowToast(toast);
        }

        public void ShowToastText03(string title, string content, string arg = null)
        {
            var toast = ToastContent.CreateToastText03(title, content, arg);
            _helper.ShowToast(toast);
        }

        public void ShowToastText04(string title, string content, string content2, string arg = null)
        {
            var toast = ToastContent.CreateToastText04(title, content, content2, arg);
            _helper.ShowToast(toast);
        }

        public void ShowToastImageAndText01(string image, string altText, string content, string arg = null)
        {
            var toast = ToastContent.CreateToastImageAndText01(image, altText, content, arg);
            _helper.ShowToast(toast);
        }

        public void ShowToastImageAndText02(string image, string altText, string title, string content, string arg = null)
        {
            var toast = ToastContent.CreateToastImageAndText02(image, altText, title, content, arg);
            _helper.ShowToast(toast);
        }

        public void ShowToastImageAndText03(string image, string altText, string title, string content, string arg = null)
        {
            var toast = ToastContent.CreateToastImageAndText03(image, altText, title, content, arg);
            _helper.ShowToast(toast);
        }

        public void ShowToastImageAndText04(string image, string altText, string title, string content, string content2, string arg = null)
        {
            var toast = ToastContent.CreateToastImageAndText04(image, altText, title, content, content2, arg);
            _helper.ShowToast(toast);
        }
    }
}