using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Template10.Controls
{
    /// <summary>
    /// Represents a control that displays data items in a vertical stack, with accompanying selected data item in more detail
    /// </summary>
    [ContentProperty(Name = nameof(Items))]
    public sealed class MasterDetailsView : ListView
    {
        public MasterDetailsView()
        {
            this.DefaultStyleKey = typeof(MasterDetailsView);
        }
    }
}
