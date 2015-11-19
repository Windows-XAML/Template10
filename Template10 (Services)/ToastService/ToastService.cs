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
            _helper.ShowToastText01(content, arg);
        }

        public void ShowToastText02(string title, string content, string arg = null)
        {
            _helper.ShowToastText02(title, content, arg);
        }

        public void ShowToastText03(string title, string content, string arg = null)
        {
            _helper.ShowToastText03(title, content, arg);
        }

        public void ShowToastText04(string title, string content, string content2, string arg = null)
        {
            _helper.ShowToastText04(title, content, content2, arg);
        }

        public void ShowToastImageAndText01(string image, string content, string arg = null)
        {
            _helper.ShowToastImageAndText01(image, content, arg);
        }

        public void ShowToastImageAndText02(string image, string title, string content, string arg = null)
        {
            _helper.ShowToastImageAndText02(image, title, content, arg);
        }

        public void ShowToastImageAndText03(string image, string title, string content, string arg = null)
        {
            _helper.ShowToastImageAndText03(image, title, content, arg);
        }

        public void ShowToastImageAndText04(string image, string title, string content, string content2, string arg = null)
        {
            _helper.ShowToastImageAndText04(image, title, content, content2, arg);
        }
    }
}