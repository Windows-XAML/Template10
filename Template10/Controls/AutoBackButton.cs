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
    public sealed class AutoBackButton : Control
    {
        #region Constants
        private const string HardwareButtonsType = "Windows.Phone.UI.Input.HardwareButtons";
        #endregion // Constants

        #region Constructors
        public AutoBackButton()
        {
            this.DefaultStyleKey = typeof(AutoBackButton);
            this.Loaded += AutoBackButton_Loaded;
            this.Unloaded += AutoBackButton_Unloaded;
        }
        #endregion // Constructors


        #region Internal Methods
        private void CalculateState()
        {
            // If this OS has a hardware back button, hide. Otherwise, hide if there is no back stack.
            if (ApiInformation.IsTypePresent(HardwareButtonsType))
            {
                Visibility = Visibility.Collapsed;
            }
            else
            {
                // Try to find the frame
                var frame = FindFrame();

                // If frame found, show or hide ourselves based on the ability to go back
                if (frame != null)
                {
                    if (frame.CanGoBack)
                    {
                        Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Visibility = Visibility.Collapsed;
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

        private void TryGoBack()
        {
            // Get the frame
            var frame = FindFrame();

            // If found and can go back, go back
            if ((frame != null) && (frame.CanGoBack))
            {
                frame.GoBack();
            }
        }
        #endregion // Internal Methods


        #region Overrides / Event Handlers

        private void AutoBackButton_Loaded(object sender, RoutedEventArgs e)
        {
            // If hardware buttons are present, subscribe
            if (ApiInformation.IsTypePresent(HardwareButtonsType))
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }

            // Always calcualte state
            CalculateState();
        }

        private void AutoBackButton_Unloaded(object sender, RoutedEventArgs e)
        {
            // If hardware buttons are present, unsubscribe
            if (ApiInformation.IsTypePresent(HardwareButtonsType))
            {
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            }
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            // Only process if not already handled
            if (!e.Handled)
            {
                // We'll handle it
                e.Handled = true;

                // Go back (assuming we have a frame)
                TryGoBack();
            }
        }

        protected override void OnApplyTemplate()
        {
            // Pass to base first
            base.OnApplyTemplate();

            // Try to find the back button
            var BackButton = GetTemplateChild("BackButton") as Button;

            // Try to subscribe to click event
            if (BackButton != null)
            {
                BackButton.Click += BackButton_Click;
            }
            else
            {
                Debug.WriteLine("WARNING: Could not find a button named BackButton in the AutoBackButton template.");
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TryGoBack();
        }
        #endregion // Overrides / Event Handlers
    }
}
