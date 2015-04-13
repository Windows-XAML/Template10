using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10.Triggers
{
    public class ApiContractTrigger : StateTriggerBase
    {
        //private variables
        private BackButtonState _backButton, _currentBackButton;
        public enum BackButtonState
        {
            Hardware = 0,
            None = 1
        }
        //public property to be set from XAML
        public BackButtonState BackButton
        {
            get
            {
                return _backButton;
            }
            set
            {
                _backButton = value;
                UpdateTrigger();
            }
        }
        //Constructor
        public ApiContractTrigger()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                _currentBackButton = BackButtonState.Hardware;
            else
                _currentBackButton = BackButtonState.None;
            UpdateTrigger();
        }
        //Evaluate and update trigger
        private void UpdateTrigger()
        {
            SetTriggerValue(_currentBackButton == _backButton);
        }
    }
}
