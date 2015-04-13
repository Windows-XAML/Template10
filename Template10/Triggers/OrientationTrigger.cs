using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Template10.Triggers
{
    public class OrientationTrigger : StateTriggerBase
    {
        //Constructor
        public OrientationTrigger()
        {
            Window.Current.SizeChanged += Current_SizeChanged;
        }
        //public properties to be set from XAML
        public ApplicationViewOrientation Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
            }
        }
        //private properties
        private ApplicationViewOrientation _orientation, _currentOrientation;
        //Handle event to get current values
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (Window.Current.Bounds.Width >= Window.Current.Bounds.Height)
            {
                _currentOrientation = ApplicationViewOrientation.Landscape;
            }
            else
            {
                _currentOrientation = ApplicationViewOrientation.Portrait;
            }
            UpdateTrigger();
        }
        //Logic to evaluate and apply trigger value
        private void UpdateTrigger()
        {
            SetTriggerValue(_currentOrientation == _orientation);
        }
    }

}
