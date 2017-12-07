using Windows.Foundation.Metadata;

namespace Template10.Extensions
{
    public static class ApiUtils
    {
        static bool? _isHardwareButtonsApiPresent;
        public static bool IsHardwareButtonsApiPresent
        {
            get
            {
                if (_isHardwareButtonsApiPresent.HasValue)
                {
                    return _isHardwareButtonsApiPresent.Value;
                }
                else
                {
                    return (_isHardwareButtonsApiPresent = ApiInformation.IsTypePresent(@"Windows.Phone.UI.Input.HardwareButtons")).Value;
                }
            }
        }
    }
}