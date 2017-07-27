using Windows.Foundation.Metadata;

namespace Template10.Extensions
{
    public static class ApiUtils
    {
        static bool? isHardwareButtonsApiPresent;
        public static bool IsHardwareButtonsApiPresent => isHardwareButtonsApiPresent ?? (isHardwareButtonsApiPresent = ApiInformation.IsTypePresent(@"Windows.Phone.UI.Input.HardwareButtons")).Value;
    }
}