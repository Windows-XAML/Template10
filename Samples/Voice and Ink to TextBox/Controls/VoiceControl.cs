using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MPC.Controls
{
    public class VoiceControl : TextBox
    {
        public VoiceControl()
        {
            this.DefaultStyleKey = typeof(VoiceControl);
        }

        #region override OnApplyTemplate
        protected override void OnApplyTemplate()
        {
            ////textBox = GetTemplateChild(PART_TEXT_NAME) as TextBox;
            //button = GetTemplateChild(PART_BUTTON_NAME) as Button;
            //this.PlaceholderText = "Type or Speech";
            ////this.Text = "Hello";
            //InitEvents();

        }
        #endregion
    }
}
