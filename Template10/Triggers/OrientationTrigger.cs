using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Template10.Triggers
{
    /// <summary>
    /// A trigger that changes state based on the orientation of the current window.
    /// </summary>
    public class OrientationTrigger : StateTriggerBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="OrientationTrigger"/> instance.
        /// </summary>
        public OrientationTrigger()
        {
            // Get application main window
            var win = Window.Current;

            // Create a weak subscription to the SizeChanged event so that we don't pin the trigger or page in memory
            WeakEvent.Subscribe<WindowSizeChangedEventHandler>(win, nameof(win.SizeChanged), Window_SizeChanged);

            // Calculate the initial state
            CalculateState();
        }
        #endregion // Constructors


        #region Internal Methods
        private void CalculateState()
        {
            var currentOrientation = ApplicationViewOrientation.Landscape;
            var window = Window.Current;
            if (window.Bounds.Width >= window.Bounds.Height)
            {
                currentOrientation = ApplicationViewOrientation.Landscape;
            }
            else
            {
                currentOrientation = ApplicationViewOrientation.Portrait;
            }
            SetActive(currentOrientation == orientation);
        }
        #endregion // Internal Methods

        #region Overrides / Event Handlers
        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            // System.Diagnostics.Debug.WriteLine(string.Format("Size Changed {0}", this.GetHashCode()));
            CalculateState();
        }
        #endregion // Overrides / Event Handlers


        #region Public Properties
        private ApplicationViewOrientation orientation;
        /// <summary>
        /// Gets or sets the orientation that will satisfy the trigger.
        /// </summary>
        /// <value>
        /// The orientation that will satisfy the trigger.
        /// </value>
        public ApplicationViewOrientation Orientation
        {
            get
            {
                return orientation;
            }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    CalculateState();
                }
            }
        }
        #endregion // Public Properties
    }
}
