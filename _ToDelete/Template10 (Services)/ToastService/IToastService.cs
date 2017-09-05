using System;
using System.Collections.Generic;
using System.Text;

namespace Template10.Services.ToastService
{
    public interface IToastService
    {
        void ShowToastText01(string content, string arg = null);
        void ShowToastText02(string title, string content, string arg = null);
        void ShowToastText03(string title, string content, string arg = null);
        void ShowToastText04(string title, string content, string content2, string arg = null);
        void ShowToastImageAndText01(string image,string altText, string content, string arg = null);
        void ShowToastImageAndText02(string image, string altText, string title, string content, string arg = null);
        void ShowToastImageAndText03(string image, string altText, string title, string content, string arg = null);
        void ShowToastImageAndText04(string image, string altText, string title, string content, string content2, string arg = null); 

    }

}
