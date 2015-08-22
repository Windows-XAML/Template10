using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml;

namespace Blank1.Triggers
{
    public class DeviceFamilyTrigger : StateTriggerBase
    {
        public string DeviceFamily
        {
            get { return string.Empty; }
            set
            {
                var family = ResourceContext.GetForCurrentView().QualifierValues["DeviceFamily"];
                SetActive(family.Equals(value));
            }
        }
    }

}
