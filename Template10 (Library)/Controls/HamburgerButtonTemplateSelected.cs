using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Controls
{
    public class HamburgerButtonTemplateSelected : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var info = item as HamburgerButtonInfo;
            switch (info.ButtonType)
            {
                case HamburgerButtonInfo.ButtonTypes.Toggle: return ToggleTemplate;
                case HamburgerButtonInfo.ButtonTypes.Command: return CommandTemplate;
                case HamburgerButtonInfo.ButtonTypes.Literal: return LiteralTemplate;
                default: return ToggleTemplate;
            }
        }
        public DataTemplate ToggleTemplate { get; set; }
        public DataTemplate CommandTemplate { get; set; }
        public DataTemplate LiteralTemplate { get; set; }
    }
}
