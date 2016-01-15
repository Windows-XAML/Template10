using System;
using Windows.Foundation.Metadata;

namespace Template10.Utils
{
    public static class ApiUtils
    {
        static readonly bool isHardwareButtonsApiPresent = ApiInformation.IsTypePresent(@"Windows.Phone.UI.Input.HardwareButtons");

        public static bool IsHardwareButtonsApiPresent => isHardwareButtonsApiPresent;
    }
}