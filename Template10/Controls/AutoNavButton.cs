using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Template10.Controls
{
    /// <summary>
    /// Defines the possible modes of the <see cref="AutoNavButton"/>.
    /// </summary>
    public enum AutoNavMode
    {
        /// <summary>
        /// The nav button will go back one page (if possible).
        /// </summary>
        Back,
        /// <summary>
        /// The nav button will go forward one page (if possible).
        /// </summary>
        Forward,
        /// <summary>
        /// The nav button will go to the home page (if possible).
        /// </summary>
        Home
    };

    /// <summary>
    /// A button that handles navigation automatically including graphically and using keyboard and mouse.
    /// </summary>
    public sealed class AutoNavButton : Control
    {
        #region Static Version
        #region Constants
        private const string DisabledStateName = "Disabled";
        private const string EnabledStateName = "Enabled";
        private const string HardwareButtonsType = "Windows.Phone.UI.Input.HardwareButtons";
        private const string NavButtonName = "NavButton";
        #endregion // Constants


        #region Dependency Property Definitions
        /// <summary>
        /// Identifies the <see cref="Mode"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(AutoNavMode), typeof(AutoNavButton), new PropertyMetadata(AutoNavMode.Back, OnModeChanged));
        #endregion // Dependency Property Definitions

        #region Dependency Property Change Forwards
        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoNavButton)d).OnModeChanged(e);
        }
        #endregion // Dependency Property Change Forwards
        #endregion // Static Version


        #region Instance Version

        #region Constructors
        public AutoNavButton()
        {
            this.DefaultStyleKey = typeof(AutoNavButton);
            this.Loaded += AutoNavButton_Loaded;
            this.Unloaded += AutoNavButton_Unloaded;
        }
        #endregion // Constructors


        #region Internal Methods
        private void CalculateState()
        {
            // Switch the visual state to match the mode
            VisualStateManager.GoToState(this, Mode.ToString(), true);

            // If Mode is Back and this OS has a hardware back button, disable. Otherwise, disable if there is no back stack.
            if ((Mode == AutoNavMode.Back) && (ApiInformation.IsTypePresent(HardwareButtonsType)))
            {
                SetEnabled(false);
            }
            // Enabled is calculated based on Mode
            else
            {
                // Try to find the frame
                var frame = FindFrame();

                // If frame found, show or hide ourselves based on the ability to navigate
                if (frame != null)
                {
                    switch (Mode)
                    {
                        case (AutoNavMode.Back):
                        case (AutoNavMode.Home):
                            if (frame.CanGoBack)
                            {
                                SetEnabled(true);
                            }
                            break;
                        case (AutoNavMode.Forward):
                            if (frame.CanGoForward)
                            {
                                SetEnabled(true);
                            }
                            break;

                        default:
                            // Unknown mode (shouldn't happen)
                            SetEnabled(false);
                            break;
                    }
                }
            }
        }

        private Frame FindFrame()
        {
            // Look for a frame in the current window
            // TODO: May need to get from NavigationService in template
            return Window.Current.Content as Frame;
        }

        private void SetEnabled(bool enabled)
        {
            // The control has to be set to collapsed by default due to a bug in layout calculation
            // When the control is collapsed, Load does not occur. Therefore when we go to enabled
            // we need to make sure we're also visible.
            if (enabled)
            {
                Visibility = Visibility.Visible;
            }

            // Which state?
            var stateName = (enabled ? EnabledStateName : DisabledStateName);

            // Go to the state
            VisualStateManager.GoToState(this, EnabledStateName, true);
        }

        private bool TryNavigate()
        {
            // Get the frame
            var frame = FindFrame();

            // If frame is found and we can perform the operation
            if (frame != null)
            {
                switch (Mode)
                {
                    case AutoNavMode.Back:
                        if (frame.CanGoBack)
                        {
                            frame.GoBack();
                            return true;
                        }
                        break;

                    case AutoNavMode.Forward:
                        if (frame.CanGoForward)
                        {
                            frame.GoForward();
                            return true;
                        }
                        break;

                    case AutoNavMode.Home:
                        if (frame.CanGoBack)
                        {
                            while (frame.CanGoBack)
                            {
                                frame.GoBack();
                            }
                            return true;
                        }
                        break;
                }

                // Shouldn't get here but in case some other mode is added
                return false;
            }
            else
            {
                return false;
            }
        }
        #endregion // Internal Methods


        #region Overrides / Event Handlers
        private void AutoNavButton_Loaded(object sender, RoutedEventArgs e)
        {
            // If hardware buttons are present, subscribe
            if (ApiInformation.IsTypePresent(HardwareButtonsType))
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }

            // Always calcualte state
            CalculateState();
        }

        private void AutoNavButton_Unloaded(object sender, RoutedEventArgs e)
        {
            // If hardware buttons are present, unsubscribe
            if (ApiInformation.IsTypePresent(HardwareButtonsType))
            {
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            }
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            TryNavigate();
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            // Only process if not already handled
            if (!e.Handled)
            {
                // Only handle if our mode is back
                if (Mode == AutoNavMode.Back)
                {
                    // We'll try to handle it, assuming we have a frame and can go back
                    e.Handled = TryNavigate();
                }
            }
        }

        protected override void OnApplyTemplate()
        {
            // Pass to base first
            base.OnApplyTemplate();

            // Try to find the buton button
            var NavButton = GetTemplateChild(AutoNavButton.NavButtonName) as Button;

            // Try to subscribe to click event
            if (NavButton != null)
            {
                NavButton.Click += NavButton_Click;
            }
            else
            {
                Debug.WriteLine(string.Format("WARNING: Could not find a button named '{0}' in the {1} template.", AutoNavButton.NavButtonName, typeof(AutoNavButton).Name));
            }

            // Update state
            CalculateState();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Mode"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        private void OnModeChanged(DependencyPropertyChangedEventArgs e)
        {
            CalculateState();
        }
        #endregion // Overrides / Event Handlers


        #region Public Properties
        /// <summary>
        /// Gets or sets the Mode of the <see cref="AutoNavButton"/>. This is a dependency property.
        /// </summary>
        /// <value>
        /// The Mode of the <see cref="AutoNavButton"/>.
        /// </value>
        public AutoNavMode Mode
        {
            get
            {
                return (AutoNavMode)GetValue(ModeProperty);
            }
            set
            {
                SetValue(ModeProperty, value);
            }
        }
        #endregion // Public Properties
        #endregion // Instance Version
    }
}
